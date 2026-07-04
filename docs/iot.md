# Bounded Context IoT (Internet of Things) - Atelier Platform

Este documento detalla la estructura, conectividad y endpoints RESTful del Bounded Context (Módulo) de IoT dentro de la plataforma Atelier.

## 1. Arquitectura y Conectividad

El Bounded Context `iot` está implementado siguiendo los principios de **Arquitectura Limpia (Clean Architecture)** y **Domain-Driven Design (DDD)**. 

### ¿Cómo se comunican los dispositivos?
A diferencia de otros sistemas IoT que mantienen conexiones persistentes usando WebSockets o protocolos M2M como MQTT o AMQP, **esta plataforma utiliza un modelo de comunicación pasiva vía HTTP/REST síncrono**.

1. **Ingesta de Datos (Telemetría):** Los dispositivos físicos OBD2 instalados en los vehículos recopilan métricas y estados (snapshots).
2. **Envío:** Periódicamente, los dispositivos agrupan estos datos y realizan una petición HTTP `POST` estándar hacia la API (específicamente al endpoint `/api/v1/telemetry-batches`).
3. **Seguridad:** Los endpoints están protegidos por el módulo de Identity and Access Management (IAM) mediante JSON Web Tokens (JWT) que los clientes/dispositivos envían en los headers de sus peticiones HTTP.

---

## 2. Endpoints de la API RESTful

El paquete de interfaces expone la funcionalidad a través de múltiples Controladores REST organizados por dominio.

### 2.1. Gestión de Dispositivos OBD2 (`/api/v1/obd2-devices`)
Controlador: `Obd2DevicesController`

Se encarga de la gestión del ciclo de vida (CRUD) del hardware físico (dispositivos OBD2) antes de que sean vinculados a un vehículo.

* **`POST /`**: Registra un nuevo dispositivo OBD2 en una sucursal (`branchId`) proporcionando su dirección MAC.
* **`GET /`**: Obtiene todos los dispositivos OBD2 de una sucursal. Soporta filtrado (ej. `?status=available` para obtener los no vinculados).
* **`GET /{id}`**: Obtiene los detalles de un dispositivo OBD2 específico por su UUID.
* **`PUT /{id}`**: Actualiza los detalles de un dispositivo (como la dirección MAC).
* **`DELETE /{id}`**: Realiza un borrado lógico (soft delete) del dispositivo.
* **`GET /{deviceId}/telemetry-snapshots/latest`**: Obtiene la última captura de telemetría (última "foto" de datos) registrada por ese dispositivo.
* **`GET /{deviceId}/telemetry-snapshots`**: Obtiene el historial completo de telemetría registrado por ese dispositivo específico ordenado descendentemente.

### 2.2. Gestión de Vehículos (`/api/v1/vehicles`)
Controlador: `VehiclesController`

Se encarga de la gestión de la flota de vehículos dentro del contexto IoT. 

* **`POST /`**: Registra un nuevo vehículo y lo vincula automáticamente al usuario/cliente autenticado (extrae el `userId` del JWT token).
* **`GET /`**: Obtiene los vehículos de una sucursal. Soporta filtrado (ej. `?status=available-for-linking` para obtener aquellos listos para instalarles un dispositivo OBD2).
* **`GET /{id}`**: Obtiene los detalles de un vehículo por su UUID.
* **`PUT /{id}`**: Actualiza los detalles de un vehículo.
* **`DELETE /{id}`**: Borrado lógico (soft delete) del vehículo. También desactiva enlaces activos con dispositivos OBD2 o conductores.
* **`GET /{vehicleId}/telemetry-snapshots`**: Obtiene el histórico de telemetría de este vehículo **desde que inició su registro activo** con el conductor actual.
* **`GET /{vehicleId}/dtc-alerts`**: Obtiene las alertas DTC (Diagnostic Trouble Codes / Fallas de motor) de este vehículo.

### 2.3. Vínculo Dispositivo-Vehículo (`/api/v1/obd2-device-registrations`)
Controlador: `Obd2DeviceRegistrationsController`

Gestiona el acoplamiento o enlace entre un dispositivo físico OBD2 y un vehículo. Es decir, cuándo se instala el dispositivo en un carro.

* **`POST /`**: Vincula un dispositivo OBD2 disponible a un vehículo dentro de una sucursal. 
* **`PATCH /{id}`**: Actualiza el estado del vínculo. Principalmente utilizado para desvincular (`status=INACTIVE`), es decir, cuando se retira el dispositivo del carro.
* **`GET /`**: Obtiene todos los vínculos registrados bajo una sucursal filtrados por estado (Activo/Inactivo).
* **`GET /{id}/telemetry-snapshots`**: Obtiene la telemetría capturada *específicamente durante el tiempo que duró este vínculo*.
* **`GET /{id}/dtc-alerts`**: Obtiene las alertas de fallas de motor capturadas *durante este vínculo*.

### 2.4. Integración de Clientes (`/api/v1/customers`)
Controlador: `CustomerVehiclesController`

Expone operaciones específicas orientadas a clientes, manteniendo las fronteras de dominio.

* **`GET /{customerId}/vehicles`**: Obtiene la lista de todos los vehículos que actualmente tienen un registro o vinculación activa para un cliente específico.

### 2.5. Ingesta de Telemetría (`/api/v1/telemetry-batches`)
Controlador: `TelemetryBatchesController`

Este es el endpoint de entrada principal para la recepción masiva de datos en tiempo real (o por lotes) generados por los vehículos.

* **`POST /`**: Ingiere un lote de capturas de telemetría enviadas por un dispositivo OBD2. El payload incluye el ID del dispositivo y la data en bruto que luego es transformada y procesada por los comandos internos del sistema.

---

## 3. Resumen del Flujo
1. Se crea un vehículo (`/vehicles`).
2. Se registra físicamente un escáner (`/obd2-devices`).
3. Se instala el escáner en el vehículo creando una vinculación (`/obd2-device-registrations`).
4. El escáner comienza a enviar ráfagas de datos periódicamente a la API HTTP REST (`/telemetry-batches`).
5. Los clientes/conductores consultan en tiempo real o histórico el estado de su vehículo y fallas de motor a través de las consultas de lectura (`/vehicles/{id}/telemetry-snapshots`, `/vehicles/{id}/dtc-alerts`).

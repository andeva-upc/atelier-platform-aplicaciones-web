# Bounded Context Fleet (Flota / Gestión Operativa) - Atelier Platform

Este documento detalla la estructura, conectividad y endpoints RESTful del Bounded Context (Módulo) de Fleet dentro de la plataforma Atelier.

## 1. Arquitectura y Conectividad

El Bounded Context `fleet` está implementado siguiendo los principios de **Arquitectura Limpia (Clean Architecture)** y **Domain-Driven Design (DDD)**. 

### ¿Cómo se comunican las aplicaciones?
Al igual que el resto de los módulos administrativos de la plataforma, **este módulo utiliza un modelo de comunicación pasiva vía HTTP/REST síncrono**. No se requiere persistencia en la conexión (como WebSockets o mensajería asíncrona).

1. **Interacción:** Aplicaciones cliente (como un portal web administrativo para el taller o una app móvil para el usuario final) realizan peticiones a la API para agendar citas o gestionar el personal.
2. **Seguridad:** Los endpoints están protegidos por el módulo de Identity and Access Management (IAM) mediante JSON Web Tokens (JWT) que los clientes envían en los headers de autorización de sus peticiones HTTP.

---

## 2. Endpoints de la API RESTful

El paquete de interfaces expone la funcionalidad a través de 3 Controladores REST principales.

### 2.1. Gestión de Citas (`/api/v1/appointments`)
Controlador: `AppointmentsController`

Permite a los clientes agendar citas para la revisión de sus vehículos y a los administradores del taller gestionar estas reservaciones.

* **`POST /`**: Crea (agenda) una nueva cita. El payload debe contener la información necesaria como fecha, `customerId`, `vehicleId`, etc.
* **`GET /`**: Lista las citas agendadas. Soporta múltiples filtros combinables a través de query params para realizar búsquedas específicas:
  * `?branchId=...` (Citas de una sucursal)
  * `?status=...` (Estado de la cita)
  * `?customerId=...` (Citas de un cliente)
  * `?vehicleId=...` (Citas de un vehículo)
* **`GET /{id}`**: Obtiene los detalles completos de una sola cita a través de su identificador UUID.
* **`PUT /{id}`**: Actualiza los detalles de una cita existente (por ejemplo, reagendar la fecha o cambiar el estado).
* **`DELETE /{id}`**: Cancela o realiza un borrado lógico (soft-delete) de la cita especificada.

### 2.2. Registro de Clientes (`/api/v1/customer-registrations`)
Controlador: `CustomerRegistrationsController`

Se encarga de dar de alta y gestionar el vínculo formal de un cliente con una sucursal (taller) en particular.

* **`POST /`**: Crea un registro de cliente, asociándolo operativamente a la plataforma del taller.
* **`GET /`**: Obtiene registros de clientes en el sistema. Soporta filtros mediante query parameters:
  * `?branchId=...`
  * `?status=...`
  * `?customerId=...`
* **`GET /{id}`**: Obtiene los detalles de un registro de cliente específico.
* **`PUT /{id}`**: Actualiza el estado u otros datos del registro de un cliente existente.
* **`DELETE /{id}`**: Desactiva (soft-delete) el registro del cliente, marcándolo como inactivo sin eliminar permanentemente su historial.

### 2.3. Registro de Empleados (`/api/v1/employee-registrations`)
Controlador: `EmployeeRegistrationsController`

Permite al administrador de la sucursal/taller dar de alta y gestionar a sus empleados (mecánicos, técnicos, etc.).

* **`POST /`**: Registra a un nuevo empleado dentro de una sucursal.
* **`GET /`**: Obtiene los registros de los empleados. Soporta filtros mediante query parameters:
  * `?branchId=...`
  * `?status=...`
  * `?employeeId=...`
* **`GET /{id}`**: Obtiene los detalles de un registro de empleado específico.
* **`PUT /{id}`**: Actualiza los datos operativos de un empleado registrado. Sirve para modificar detalles como su especialidad técnica o su salario actual (`UpdateEmployeeRegistrationResource`).
* **`DELETE /{id}`**: Desactiva al empleado (soft-delete), finalizando su vinculación activa con la sucursal.

---

## 3. Resumen del Flujo Operativo (Workflow)

El flujo típico de interacciones en el módulo Fleet (Administración Operativa) es el siguiente:

1. **Setup del Taller:** El taller o sucursal utiliza el endpoint `/employee-registrations` para registrar a todo su personal y mecánicos en el sistema.
2. **Onboarding de Usuario:** Cuando llega un conductor/cliente, se le registra formalmente en el taller usando el endpoint `/customer-registrations`.
3. **Reserva de Servicios:** El conductor (desde su app) o el administrador del taller (desde el panel web) utiliza el endpoint `/appointments` para reservar un turno. 
   - En este paso se relacionan las entidades indicando qué cliente es (`customerId`), con qué carro asiste (`vehicleId`), y en qué taller será atendido (`branchId`).
4. **Actualización de Estado:** A medida que la cita progresa (Pendiente, En Curso, Completada, Cancelada), se envían peticiones `PUT` a `/appointments/{id}` para reflejar la realidad operativa del taller.

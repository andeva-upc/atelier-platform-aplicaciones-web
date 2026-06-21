# Documentación Exhaustiva de Endpoints - Fleet & Billing Bounded Contexts

Esta guía recopila TODOS y CADA UNO de los endpoints implementados en los controladores de **Fleet** (15 endpoints) y **Billing** (11 endpoints), con ejemplos exactos de los payloads JSON que reciben y devuelven.

---

## 🚚 Bounded Context: FLEET (15 Endpoints)

### 1. Appointments (Citas) - 5 Endpoints

#### `POST /api/v1/appointments`
- **Recibe (Body):**
```json
{
  "branchId": "d4e15ef8-6ea8-4c95-88d6-9e76f1fb57d5",
  "customerId": "c1b2a3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d",
  "vehicleId": "v1b2c3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d",
  "scheduledStart": "2026-06-25T10:00:00Z",
  "scheduledEnd": "2026-06-25T12:00:00Z"
}
```
- **Devuelve (201 Created):**
```json
{
  "id": "e2f8c5b1-9a7d-4c3e-8b1a-6d5f4e3c2b1a",
  "branchId": "d4e15ef8-6ea8-4c95-88d6-9e76f1fb57d5",
  "customerId": "c1b2a3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d",
  "vehicleId": "v1b2c3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d",
  "scheduledStart": "2026-06-25T10:00:00Z",
  "scheduledEnd": "2026-06-25T12:00:00Z",
  "status": "PENDING"
}
```

#### `GET /api/v1/appointments`
- **Recibe (Query Params):** `?branchId=uuid&status=PENDING&customerId=uuid&vehicleId=uuid` (Opcionales)
- **Devuelve (200 OK):** `[ { ...objeto Appointment... } ]`

#### `GET /api/v1/appointments/{appointmentId}`
- **Recibe:** Vacío (ID en la ruta)
- **Devuelve (200 OK):** Objeto Appointment

#### `PUT /api/v1/appointments/{appointmentId}`
- **Recibe (Body):**
```json
{
  "scheduledStart": "2026-06-25T14:00:00Z",
  "scheduledEnd": "2026-06-25T16:00:00Z",
  "status": "CONFIRMED"
}
```
- **Devuelve (200 OK):** Objeto Appointment actualizado.

#### `DELETE /api/v1/appointments/{appointmentId}`
- **Recibe:** Vacío (ID en la ruta)
- **Devuelve (200 OK):** Vacío.

---

### 2. Employee Registrations (Registros de Empleados) - 5 Endpoints

#### `POST /api/v1/employee-registrations`
- **Recibe (Body):**
```json
{
  "employeeId": "e1b2c3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d",
  "branchId": "d4e15ef8-6ea8-4c95-88d6-9e76f1fb57d5",
  "speciality": "MECHANIC",
  "salary": 1500.00
}
```
- **Devuelve (201 Created):**
```json
{
  "id": "e2f8c5b1-9a7d-4c3e-8b1a-6d5f4e3c2b1a",
  "employeeId": "e1b2c3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d",
  "branchId": "d4e15ef8-6ea8-4c95-88d6-9e76f1fb57d5",
  "speciality": "MECHANIC",
  "specialityName": "Mecánico",
  "salary": 1500.00,
  "status": "ACTIVE",
  "createdAt": "2026-06-01T10:00:00Z",
  "updatedAt": "2026-06-01T10:00:00Z",
  "deletedAt": null
}
```

#### `GET /api/v1/employee-registrations`
- **Recibe (Query Params):** `?branchId=uuid&status=ACTIVE&employeeId=uuid`
- **Devuelve (200 OK):** `[ { ...objeto EmployeeRegistration... } ]`

#### `GET /api/v1/employee-registrations/{id}`
- **Recibe:** Vacío (ID en la ruta)
- **Devuelve (200 OK):** Objeto EmployeeRegistration.

#### `PUT /api/v1/employee-registrations/{id}`
- **Recibe (Body):**
```json
{
  "speciality": "MASTER_MECHANIC",
  "salary": 2000.00,
  "status": "ACTIVE"
}
```
- **Devuelve (200 OK):** Objeto EmployeeRegistration actualizado.

#### `DELETE /api/v1/employee-registrations/{id}`
- **Recibe:** Vacío (ID en la ruta)
- **Devuelve (200 OK):** Vacío.

---

### 3. Customer Registrations (Registros de Clientes) - 5 Endpoints

#### `POST /api/v1/customer-registrations`
- **Recibe (Body):**
```json
{
  "customerId": "c1b2a3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d",
  "branchId": "d4e15ef8-6ea8-4c95-88d6-9e76f1fb57d5"
}
```
- **Devuelve (201 Created):**
```json
{
  "id": "crf8c5b1-9a7d-4c3e-8b1a-6d5f4e3c2b1a",
  "customerId": "c1b2a3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d",
  "branchId": "d4e15ef8-6ea8-4c95-88d6-9e76f1fb57d5",
  "status": "ACTIVE",
  "createdAt": "2026-06-01T10:00:00Z",
  "updatedAt": "2026-06-01T10:00:00Z",
  "deletedAt": null
}
```

#### `GET /api/v1/customer-registrations`
- **Recibe (Query Params):** `?branchId=uuid&customerId=uuid`
- **Devuelve (200 OK):** `[ { ...objeto CustomerRegistration... } ]`

#### `GET /api/v1/customer-registrations/{registrationId}`
- **Recibe:** Vacío (ID en la ruta)
- **Devuelve (200 OK):** Objeto CustomerRegistration.

#### `PUT /api/v1/customer-registrations/{registrationId}`
- **Recibe (Body):**
```json
{
  "status": "INACTIVE"
}
```
- **Devuelve (200 OK):** Objeto CustomerRegistration actualizado.

#### `DELETE /api/v1/customer-registrations/{registrationId}`
- **Recibe:** Vacío (ID en la ruta)
- **Devuelve (200 OK):** Vacío.

---
---

## 💳 Bounded Context: BILLING (11 Endpoints)

### 1. Quotes (Cotizaciones) - 6 Endpoints

#### `POST /api/v1/quotes`
- **Recibe (Body):**
```json
{
  "branchId": "d4e15ef8-6ea8-4c95-88d6-9e76f1fb57d5",
  "customerId": "c1b2a3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d",
  "totalAmount": 250.50,
  "items": [
    {
      "productId": "p1b2c3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d",
      "quantity": 2,
      "unitPrice": 125.25
    }
  ]
}
```
- **Devuelve (201 Created):**
```json
{
  "id": "q2f8c5b1-9a7d-4c3e-8b1a-6d5f4e3c2b1a",
  "branchId": "d4e15ef8-6ea8-4c95-88d6-9e76f1fb57d5",
  "customerId": "c1b2a3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d",
  "totalAmount": 250.50,
  "status": "PENDING",
  "items": [
    {
      "productId": "p1b2c3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d",
      "quantity": 2,
      "unitPrice": 125.25
    }
  ],
  "createdAt": "2026-06-21T10:00:00Z"
}
```

#### `GET /api/v1/quotes?branchId={branchId}`
- **Recibe (Query Params):** `?branchId=uuid`
- **Devuelve (200 OK):** `[ { ...objeto Quote... } ]`

#### `GET /api/v1/quotes/{id}`
- **Recibe:** Vacío (ID en la ruta)
- **Devuelve (200 OK):** Objeto Quote.

#### `PUT /api/v1/quotes/{id}`
- **Recibe (Body):**
```json
{
  "totalAmount": 300.00,
  "items": [
    {
      "productId": "p1b2c3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d",
      "quantity": 3,
      "unitPrice": 100.00
    }
  ]
}
```
- **Devuelve (200 OK):** Objeto Quote actualizado.

#### `POST /api/v1/quotes/{id}/approvals`
- **Recibe:** Vacío (Body vacío)
- **Devuelve (200 OK):** Objeto Quote con `"status": "APPROVED"`.

#### `POST /api/v1/quotes/{id}/cancellations`
- **Recibe:** Vacío (Body vacío)
- **Devuelve (200 OK):** Objeto Quote con `"status": "CANCELLED"`.

---

### 2. Vouchers (Comprobantes) - 4 Endpoints

#### `POST /api/v1/vouchers`
- **Recibe (Body):**
```json
{
  "quoteId": "q2f8c5b1-9a7d-4c3e-8b1a-6d5f4e3c2b1a",
  "customerId": "c1b2a3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d",
  "amount": 250.50,
  "type": "INVOICE"
}
```
- **Devuelve (201 Created):**
```json
{
  "id": "v2f8c5b1-9a7d-4c3e-8b1a-6d5f4e3c2b1a",
  "quoteId": "q2f8c5b1-9a7d-4c3e-8b1a-6d5f4e3c2b1a",
  "customerId": "c1b2a3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d",
  "totalAmount": 250.50,
  "type": "INVOICE",
  "status": "UNPAID",
  "createdAt": "2026-06-21T11:00:00Z"
}
```

#### `GET /api/v1/vouchers?branchId={branchId}`
- **Recibe (Query Params):** `?branchId=uuid`
- **Devuelve (200 OK):** `[ { ...objeto Voucher... } ]`

#### `GET /api/v1/vouchers/{voucherId}`
- **Recibe:** Vacío (ID en la ruta)
- **Devuelve (200 OK):** Objeto Voucher.

#### `POST /api/v1/vouchers/{voucherId}/payments`
- **Recibe (Body):**
```json
{
  "amountPaid": 250.50,
  "paymentMethod": "CARD"
}
```
- **Devuelve (200 OK):** Objeto Voucher con `"status": "PAID"`.

---

### 3. Checkouts (Caja) - 1 Endpoint

#### `POST /api/v1/checkouts`
- **Recibe (Body):**
```json
{
  "branchId": "d4e15ef8-6ea8-4c95-88d6-9e76f1fb57d5",
  "customerId": "c1b2a3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d",
  "items": [
    {
      "description": "Cambio de Aceite",
      "quantity": 1,
      "price": 100.00
    }
  ]
}
```
- **Devuelve (201 Created):**
```json
{
  "checkoutId": "c2f8c5b1-9a7d-4c3e-8b1a-6d5f4e3c2b1a",
  "status": "INITIATED",
  "paymentUrl": "https://gateway.pago.com/checkout/c2f8c5b1"
}
```
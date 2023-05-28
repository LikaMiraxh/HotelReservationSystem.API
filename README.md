# Hotel Reservation API

This API provides a comprehensive system to manage a hotel, offering a wide range of endpoints to handle customers, rooms, reservations, reviews, and authentication.

## Controllers
- CustomersController
- ReservationsController
- ReviewsController
- RoomsController
- AuthenticateController

## Endpoints

### CustomersController
- `GET /api/Customers`: Get all customers
- `GET /api/Customers/{id}`: Get a customer by their ID
- `GET /api/Customers/SearchCustomers`: Search for customers with optional sorting and pagination
- `GET /api/Customers/{id}/reservations`: Get all reservations for a specific customer
- `POST /api/Customers`: Create a new customer
- `PUT /api/Customers/{id}`: Update a customer's details
- `DELETE /api/Customers/{id}`: Delete a customer

### ReservationsController
- `GET /api/Reservations`: Get all reservations
- `GET /api/Reservations/customer/{customerId}`: Get all reservations by customer ID
- `GET /api/Reservations/{id}`: Get a reservation by its ID
- `GET /api/Reservations/SearchReservations`: Search for reservations within a certain date range, with optional sorting and pagination
- `POST /api/Reservations`: Create a new reservation
- `PUT /api/Reservations/{id}`: Update a reservation
- `DELETE /api/Reservations/{id}`: Delete a reservation

### ReviewsController
- `GET /api/Reviews`: Get all reviews
- `GET /api/Reviews/room/{roomId}`: Get all reviews for a specific room
- `GET /api/Reviews/{id}`: Get a review by its ID
- `POST /api/Reviews`: Create a new review
- `PUT /api/Reviews/{id}`: Update a review
- `DELETE /api/Reviews/{id}`: Delete a review

### RoomsController
- `GET /api/Rooms`: Get all rooms
- `GET /api/Rooms/SearchRooms`: Search for rooms with optional sorting and pagination
- `GET /api/Rooms/{id}`: Get a room by its ID
- `POST /api/Rooms`: Create a new room
- `PUT /api/Rooms/{id}`: Update a room
- `DELETE /api/Rooms/{id}`: Delete a room

### AuthenticateController
- `POST /api/Authenticate`: Login with a username and password

## Authentication
The Authenticate controller handles JWT-based authentication. Users can log in to the system by sending a POST request to `/api/Authenticate`. A valid JWT token is returned upon successful login, which must be included in the Authorization header for most other API endpoints.

## Roles and Permissions
Different endpoints require different user roles.

- User Role:
  - View own reservations.
  - Create, update, and delete own reservations and reviews.

- Administrator Role:
  - View, create, update, and delete all customers, rooms, and reservations.

## Built With
- .NET 6
- ASP.NET Core 6
- Entity Framework Core 6

## Installation
You can clone this repository and run it locally on your machine. Once cloned, you can open the project in Visual Studio, restore the NuGet packages, and then build and run the project.

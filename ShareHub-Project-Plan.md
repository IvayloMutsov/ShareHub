# ShareHub – Development Project Plan

**Project:** ShareHub – Community Resource Sharing Platform
**Type:** Full-stack web application

| Component | Technology |
|---|---|
| Backend | ASP.NET Core Web API |
| Frontend | ASP.NET Core MVC |
| Database | Microsoft SQL Server |
| ORM | Entity Framework Core |
| Testing | NUnit, Moq, ASP.NET Core Integration Testing |
| Hosting | MonsterASP |
| CI/CD | GitHub Actions |
| IDE | Visual Studio 2026 |
| Version control | Git / GitHub |

---

## 0. Project objectives

### Main goal

Develop a platform where registered users can list personal belongings and allow other users to request to borrow them.

The application must allow users to:

- Register and authenticate.
- Create and manage items.
- Browse and search available items.
- Request item reservations.
- Approve or reject borrowing requests.
- Track reservation statuses.
- Complete reservations.
- Leave reviews after completed reservations.

Use a layered architecture with separate frontend and backend projects.

## Project responsibilities

### ShareHub.API

Responsible for:

- Business logic.
- Data access.
- Database operations.
- Input validation.
- Authentication and authorization.
- HTTP responses.

### ShareHub.MVC

Responsible for:

- Razor Views.
- HTML forms.
- Navigation.
- Displaying data.
- Calling the API using `HttpClient`.
- Handling frontend validation and errors.

### ShareHub.Contracts

Contains DTOs shared between MVC and API.

Examples:

- `ItemDto`
- `CreateItemRequest`
- `UpdateItemRequest`
- `ReservationDto`
- `CreateReservationRequest`
- `CategoryDto`

> **Do not share Entity Framework entities with MVC.**

### Testing projects

- **API.Tests:** unit tests for API services and business logic.
- **MVC.Tests:** unit tests for MVC controllers and frontend services.
- **IntegrationTests:** test actual HTTP endpoints and database interactions using a test environment.

---

## EPIC 1 – Solution setup and architecture

### SH-001: Create Visual Studio solution

**Priority:** Critical | **Estimate:** 1 hour

**Tasks**

- Create an empty solution named `ShareHub`.
- Create an ASP.NET Core Web API project named `ShareHub.API`.
- Create an ASP.NET Core Web App (Model-View-Controller) project named `ShareHub.MVC`.
- Create a Class Library project named `ShareHub.Contracts`.
- Configure project references:
  - API references Contracts.
  - MVC references Contracts.
- Create `src` and `tests` folders.
- Move application projects into `src`.
- Create the initial GitHub repository.

**Acceptance criteria**

- Solution builds successfully.
- API starts independently.
- MVC starts independently.
- Contracts project is referenced by both applications.
- No unnecessary project references exist.

---

### SH-002: Configure development and production environments

**Priority:** Critical | **Estimate:** 2 hours

**Tasks**

Configure the following files:

**API:**

- `appsettings.json`
- `appsettings.Development.json`
- `appsettings.Production.json`
- `Properties/launchSettings.json`

**MVC:**

- `appsettings.json`
- `appsettings.Development.json`
- `appsettings.Production.json`
- `Properties/launchSettings.json`

**Development configuration**

- Configure the API to use localhost port `7001`.
- Configure MVC to use localhost port `7002`.
- Configure the development database connection string.
- Configure the MVC API `BaseUrl` to point to the local API.
- Ensure development settings are not committed if they contain secrets.

**Production configuration**

- Configure the production API `BaseUrl`.
- Configure the production database connection string securely.
- Ensure production configuration does not contain localhost addresses.
- Keep production credentials outside source control.

**Acceptance criteria**

- Development and production configurations are separated.
- MVC can communicate with the localhost API.
- API connects to the development database.
- No production credentials are committed to Git.

---

### SH-003: Configure multiple startup projects

**Priority:** High | **Estimate:** 30 minutes

**Tasks**

- Open Solution Properties.
- Select Startup Project.
- Choose Multiple Startup Projects.
- Set `ShareHub.API` to Start.
- Set `ShareHub.MVC` to Start.
- Configure the API to use port `7001`.
- Configure MVC to use port `7002`.

**Acceptance criteria**

- Starting the solution launches both projects.
- API Swagger is accessible locally.
- MVC homepage is accessible locally.
- MVC can successfully make a request to API.

---

## EPIC 2 – Database and Entity Framework Core

### SH-004: Create database and configure EF Core

**Priority:** Critical | **Estimate:** 2 hours

**Tasks**

- Install SQL Server Developer Edition or SQL Server Express.
- Create a local database named `ShareHub_Development`.
- Install Entity Framework Core packages in the API project.
- Install the SQL Server provider.
- Install EF Core Tools.
- Install EF Core Design package.
- Create a `Data` folder.
- Create `ShareHubDbContext`.
- Configure `DbContext`.
- Register `DbContext` through dependency injection.
- Configure the development database connection string.
- Create the initial EF Core migration.
- Apply the migration to the development database.

**Acceptance criteria**

- Local SQL Server database exists.
- `DbContext` connects successfully.
- Initial migration runs successfully.
- Database tables are created.
- Application can retrieve records from the database.

---

### SH-005: Create database entities

**Priority:** Critical | **Estimate:** 3 hours

**Tasks**

Create an `ApplicationUser` entity using ASP.NET Core Identity.

`ApplicationUser` properties:

- `Id`
- `UserName`
- `Email`
- `FirstName`
- `LastName`
- `CreatedAt`

Create a `Category` entity.

`Category` properties:

- `Id`
- `Name`
- `Description`

Create an `Item` entity.

`Item` properties:

- `Id`
- `Name`
- `Description`
- `ImageUrl`
- `OwnerId`
- `CategoryId`
- `IsActive`
- `CreatedAt`

Create a `Reservation` entity.

`Reservation` properties:

- `Id`
- `ItemId`
- `BorrowerId`
- `StartDate`
- `EndDate`
- `Status`
- `CreatedAt`
- `UpdatedAt`

Create a `Review` entity.

`Review` properties:

- `Id`
- `ReservationId`
- `ReviewerId`
- `Rating`
- `Comment`
- `CreatedAt`

Configure entity relationships:

- One user can own multiple items.
- One category can contain multiple items.
- One item can have multiple reservations.
- One user can create multiple reservations.
- One reservation can have at most one review.

Then:

- Configure primary keys and foreign keys.
- Configure required properties.
- Configure appropriate delete behaviors.
- Create and apply a new EF Core migration.

**Acceptance criteria**

- All entities are created.
- Entity relationships are configured.
- Primary and foreign keys are correct.
- Required properties are configured.
- Database migration is successful.
- Historical reservation records are protected from accidental deletion.

---

## EPIC 3 – API architecture

### SH-006: Create DTOs and API contracts

**Priority:** High | **Estimate:** 2 hours

**Tasks**

Create DTO folders in `ShareHub.Contracts`.

**Items**

- `ItemDto`
- `CreateItemRequest`
- `UpdateItemRequest`

**Reservations**

- `ReservationDto`
- `CreateReservationRequest`
- `UpdateReservationRequest`

**Categories**

- `CategoryDto`
- `CreateCategoryRequest`
- `UpdateCategoryRequest`

**Users**

- `UserDto`
- `RegisterRequest`
- `LoginRequest`

Then:

- Add validation attributes where appropriate.
- Separate request DTOs from response DTOs.
- Avoid exposing Entity Framework entities through API responses.
- Never expose password hashes or internal authentication information.

**Acceptance criteria**

- All required DTOs exist.
- Request and response models are separated.
- DTOs contain only required information.
- Validation rules are configured.

---

### SH-007: Implement dependency injection and service architecture

**Priority:** High | **Estimate:** 3 hours

**Tasks**

Create service interfaces:

- `IItemService`
- `IReservationService`
- `ICategoryService`

Create repository interfaces:

- `IItemRepository`
- `IReservationRepository`
- `ICategoryRepository`

Create repository implementations:

- `ItemRepository`
- `ReservationRepository`
- `CategoryRepository`

Create service implementations:

- `ItemService`
- `ReservationService`
- `CategoryService`

**Repository responsibilities**

- Retrieve database records.
- Insert records.
- Update records.
- Delete records where permitted.

**Service responsibilities**

- Implement business rules.
- Validate operations.
- Coordinate repository operations.
- Return meaningful results.

Then:

- Register services in `Program.cs`.
- Register repositories in `Program.cs`.
- Use constructor dependency injection.
- Avoid accessing `DbContext` directly from controllers.

**Acceptance criteria**

- Controllers depend on services.
- Services depend on repositories.
- Repositories use `DbContext`.
- Dependencies are registered.
- Business logic is not implemented directly in controllers.

---

## EPIC 4 – Item management API

### SH-008: Implement item CRUD endpoints

**Priority:** Critical | **Estimate:** 4 hours

**Tasks**

Create `ItemsController`.

Implement the following endpoints:

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/items` | Retrieve all active items. |
| GET | `/api/items/{id}` | Retrieve an item by ID. |
| POST | `/api/items` | Create a new item. |
| PUT | `/api/items/{id}` | Update an existing item. |
| DELETE | `/api/items/{id}` | Deactivate an item. |

Then:

- Validate incoming DTOs.
- Return appropriate HTTP status codes.
- Handle non-existing items.
- Prevent unauthorized modifications.
- Ensure users can only modify their own items.
- Test endpoints through Swagger.

**Acceptance criteria**

- All CRUD operations work.
- Invalid input is rejected.
- Missing items return 404.
- Database changes are persisted.
- Unauthorized modifications are rejected.
- Swagger displays all endpoints.

---

### SH-009: Implement category API

**Priority:** Medium | **Estimate:** 2 hours

**Tasks**

Create `CategoriesController`.

Implement the following endpoints:

- `GET /api/categories`
- `GET /api/categories/{id}`
- `POST /api/categories`
- `PUT /api/categories/{id}`

Then:

- Implement category retrieval.
- Implement category creation.
- Implement category editing.
- Prevent duplicate category names.
- Prevent invalid category references.
- Ensure category management is restricted to administrators.

**Acceptance criteria**

- Categories can be created and retrieved.
- Duplicate categories are rejected.
- Items can reference valid categories.
- Unauthorized users cannot manage categories.

---

## EPIC 5 – MVC and API communication

### SH-010: Configure HttpClient

**Priority:** Critical | **Estimate:** 2 hours

**Tasks**

Create MVC service interfaces:

- `IItemApiService`
- `ICategoryApiService`

Create MVC service implementations:

- `ItemApiService`
- `CategoryApiService`

Then:

- Configure typed `HttpClient` services.
- Read the API `BaseUrl` from application configuration.
- Implement GET requests.
- Implement POST requests.
- Implement PUT requests.
- Implement DELETE requests.
- Deserialize JSON responses into DTOs.
- Handle unsuccessful HTTP responses.
- Handle API connection failures.
- Configure reasonable request timeouts.
- Avoid hardcoding API URLs.
- Use dependency injection.
- Avoid creating a new `HttpClient` for every request.

**Acceptance criteria**

- MVC can call the API.
- GET requests return DTOs.
- POST requests create database records.
- PUT requests update records.
- DELETE requests work.
- Connection failures are handled gracefully.

---

### SH-011: Create MVC item catalogue

**Priority:** High | **Estimate:** 3 hours

**Tasks**

Create `ItemsController` in MVC.

Create the following Razor Views:

- `Index.cshtml`
- `Details.cshtml`
- `Create.cshtml`
- `Edit.cshtml`

Then:

- Create the catalogue page.
- Display items using Bootstrap.
- Create an item details page.
- Create an item creation form.
- Create an item editing form.
- Implement form validation.
- Display API error messages.
- Add navigation between pages.
- Use ViewModels where appropriate.

**Acceptance criteria**

- Items are loaded from API.
- Catalogue displays item information.
- Details page works.
- Items can be created through MVC.
- Items can be edited through MVC.
- Validation errors are visible.

---

### SH-012: Implement search and filtering

**Priority:** Medium | **Estimate:** 2 hours

**Tasks**

Extend the API item endpoint to support query parameters:

- Search by item name.
- Filter by category.
- Implement pagination.
- Validate pagination parameters.

Example endpoint:

```http
GET /api/items?search=drill&categoryId=2&page=1&pageSize=10
```

Then:

- Add a search form to MVC.
- Add a category dropdown.
- Add pagination controls.
- Display search results.

**Acceptance criteria**

- Search returns matching items.
- Category filtering works.
- Pagination works.
- Invalid pagination values are handled.
- Search filters can be combined.

---

## EPIC 6 – Authentication and authorization

### SH-013: Implement ASP.NET Core Identity

**Priority:** Critical | **Estimate:** 5 hours

**Tasks**

- Configure ASP.NET Core Identity with SQL Server.
- Create `ApplicationUser`.
- Configure password requirements.
- Implement user registration.
- Implement login.
- Implement logout.
- Implement JWT bearer authentication for the API.
- Create `User` and `Admin` roles.
- Configure authorization policies.
- Configure secure password storage.
- Use official ASP.NET Core authentication libraries.
- Never implement custom password hashing or token generation algorithms.
- Configure token expiration.
- Configure appropriate token validation.

**Acceptance criteria**

- Users can register.
- Users can log in.
- Invalid credentials are rejected.
- Passwords are securely hashed.
- Protected API endpoints require authentication.
- Admin endpoints require appropriate roles.

---

### SH-014: Integrate authentication into MVC

**Priority:** High | **Estimate:** 4 hours

**Tasks**

- Create a login page.
- Create a registration page.
- Implement MVC authentication handling.
- Store authentication tokens server-side.
- Use secure HttpOnly cookies for MVC sessions.
- Attach API bearer tokens to authorized `HttpClient` requests.
- Implement logout.
- Protect MVC routes.
- Add authorization checks.
- Add CSRF protection to MVC form submissions.
- Never expose API secrets in Razor Views.
- Ensure authenticated identity is validated.

**Acceptance criteria**

- Registration works through MVC.
- Login works through MVC.
- Authenticated users can access protected pages.
- Anonymous users are redirected to login.
- Users cannot access another user's private resources.
- Logout ends the MVC session.

---

### SH-015: Implement user profile management

**Priority:** Medium | **Estimate:** 2 hours

**Tasks**

Create user profile API endpoints:

- `GET /api/users/me`
- `PUT /api/users/me`

Then:

- Create MVC profile page.
- Display the authenticated user's information.
- Allow users to update their first and last name.
- Validate profile updates.
- Prevent users from modifying another user's profile.

**Acceptance criteria**

- Users can view their profile.
- Users can update their profile.
- Invalid information is rejected.
- Users cannot modify another user's profile.

---

## EPIC 7 – Reservation system

### SH-016: Implement reservation business logic

**Priority:** Critical | **Estimate:** 5 hours

**Tasks**

- Create `ReservationService`.
- Create `ReservationsController`.

Create reservation statuses:

- `Pending`
- `Approved`
- `Rejected`
- `Cancelled`
- `Completed`

Implement the following endpoints:

- `POST /api/reservations`
- `GET /api/reservations/my`
- `GET /api/reservations/received`
- `GET /api/reservations/{id}`
- `PUT /api/reservations/{id}/approve`
- `PUT /api/reservations/{id}/reject`
- `PUT /api/reservations/{id}/cancel`
- `PUT /api/reservations/{id}/complete`

Implement the following business rules:

- A user cannot borrow their own item.
- Start date must be before end date.
- Start date cannot be in the past.
- Inactive items cannot be reserved.
- Only the owner can approve or reject requests.
- Only the borrower can cancel their own pending request.
- Only approved reservations can be completed.
- An item cannot have overlapping approved reservations.
- Completed reservations cannot be modified.
- Users cannot approve reservations belonging to another owner.

Then:

- Implement date-overlap validation.
- Implement reservation status transitions.
- Handle simultaneous reservation requests safely.
- Use appropriate transaction boundaries.
- Prevent race conditions when approving reservations.

**Acceptance criteria**

- Reservation requests work.
- Invalid dates are rejected.
- Users cannot reserve their own items.
- Owners can approve and reject requests.
- Overlapping approved reservations are prevented.
- Unauthorized users cannot modify reservations.
- Status transitions are validated.

---

### SH-017: Create MVC reservation pages

**Priority:** High | **Estimate:** 4 hours

**Tasks**

Create the following Razor Views:

- `MyReservations.cshtml`
- `ReceivedRequests.cshtml`
- `Details.cshtml`

Then:

- Add a borrow button to item details.
- Create a reservation form.
- Display the user's reservation requests.
- Display incoming requests for owned items.
- Add approve and reject buttons.
- Add a cancel button.
- Add a complete button.
- Display reservation statuses.
- Display reservation date ranges.
- Display appropriate actions depending on reservation status and user permissions.

**Acceptance criteria**

- Users can request items.
- Owners can manage incoming requests.
- Users can view reservation history.
- Buttons are displayed only when appropriate.
- API authorization remains the final authority.

---

## EPIC 8 – Reviews and favourites

### SH-018: Implement reviews

**Priority:** Medium | **Estimate:** 3 hours

**Tasks**

- Create `ReviewsController`.
- Create `ReviewService`.
- Create review repository.

Implement the following endpoints:

- `POST /api/reviews`
- `GET /api/items/{id}/reviews`

Implement the following business rules:

- Only completed reservations can be reviewed.
- A reservation can only have one review.
- Rating must be between 1 and 5.
- Reviewer must be the borrower.
- Reviews cannot be submitted for another user's reservation.

Then:

- Create MVC review form.
- Display reviews on item details pages.
- Display average item rating.

**Acceptance criteria**

- Reviews can be submitted.
- Invalid ratings are rejected.
- Duplicate reviews are rejected.
- Item reviews can be displayed.
- Average ratings are calculated correctly.

---

### SH-019: Implement favourites

**Priority:** Low | **Estimate:** 2 hours

**Tasks**

Create `UserFavourite` entity.

Properties:

- `Id`
- `UserId`
- `ItemId`
- `CreatedAt`

Then:

- Create `FavouritesController`.
- Create `FavouriteService`.
- Create favourite repository.

Implement the following endpoints:

- `POST /api/favourites/{itemId}`
- `DELETE /api/favourites/{itemId}`
- `GET /api/favourites`

Then:

- Create a favourites page in MVC.
- Add a favourite button to item details.
- Allow users to remove favourites.
- Prevent duplicate favourites.

**Acceptance criteria**

- Users can favourite items.
- Users can remove favourites.
- Users can view favourite items.
- Duplicate favourites are prevented.
- Only authenticated users can manage favourites.

---

## EPIC 9 – Administration and dashboard

### SH-020: Create admin dashboard

**Priority:** Medium | **Estimate:** 3 hours

**Tasks**

Create an admin dashboard API endpoint:

- `GET /api/admin/dashboard`

Display the following statistics:

- Total registered users.
- Total listed items.
- Total active reservations.
- Total completed reservations.
- Most popular categories.

Then:

- Implement statistics queries.
- Create dashboard ViewModel.
- Create MVC admin dashboard page.
- Display statistics using Bootstrap.
- Restrict dashboard to administrators.
- Ensure regular users cannot access administrative functionality.

**Acceptance criteria**

- Admin can access dashboard.
- Normal users cannot access dashboard.
- Statistics are retrieved from API.
- Dashboard displays correct counts.

---

### SH-021: Implement category administration

**Priority:** Low | **Estimate:** 2 hours

**Tasks**

- Create an admin category management page.
- Display all categories.
- Allow administrators to create categories.
- Allow administrators to edit categories.
- Prevent duplicate category names.
- Prevent deletion of categories that are still being used by items.
- Restrict all category management endpoints to administrators.

**Acceptance criteria**

- Administrators can manage categories.
- Normal users cannot manage categories.
- Categories in use cannot be deleted.
- Validation errors are displayed.

---

## EPIC 10 – Unit testing with NUnit

### SH-022: Configure NUnit test projects

**Priority:** Critical | **Estimate:** 2 hours

**Tasks**

Create the following test projects:

- `ShareHub.API.Tests`
- `ShareHub.MVC.Tests`
- `ShareHub.IntegrationTests`

Install the following packages:

- `NUnit`
- `NUnit3TestAdapter`
- `Microsoft.NET.Test.Sdk`
- `Moq`
- `Microsoft.AspNetCore.Mvc.Testing`

Then:

- Configure project references.
- Configure test discovery in Visual Studio.
- Verify tests run through Test Explorer.
- Verify tests run through `dotnet test`.
- Ensure tests do not use the production database.

**Acceptance criteria**

- All test projects build.
- Visual Studio detects NUnit tests.
- `dotnet test` executes tests.
- Tests run independently of the production database.

---

### SH-023: Write unit tests for ItemService

**Priority:** High | **Estimate:** 3 hours

**Tasks**

Use Moq to mock repository dependencies.

Test the following scenarios:

- Existing item is returned.
- Missing item returns null.
- Invalid item creation is rejected.
- Repository methods are called correctly.
- Inactive items cannot be reserved.
- Item updates work correctly.
- Unauthorized operations are rejected where applicable.

Then:

- Follow the Arrange, Act, Assert testing pattern.
- Use meaningful test names.
- Keep tests independent of one another.

**Acceptance criteria**

- `ItemService` has unit test coverage.
- Dependencies are mocked.
- Tests follow Arrange, Act, Assert.
- All tests pass.

---

### SH-024: Write reservation business logic tests

**Priority:** Critical | **Estimate:** 4 hours

**Tasks**

Test the following scenarios:

- Valid reservation is accepted.
- Start date in the past is rejected.
- Start date after end date is rejected.
- Borrowing own item is rejected.
- Inactive item reservation is rejected.
- Overlapping approved reservation is rejected.
- Non-overlapping reservation is accepted.
- Unauthorized approval is rejected.
- Invalid status transition is rejected.
- Adjacent reservations do not overlap.
- Identical reservation dates overlap.
- Reservation completely inside another reservation overlaps.
- Reservation containing another reservation overlaps.

Prefer testing the actual `ReservationService` rather than testing only the date-overlap calculation.

**Acceptance criteria**

- All reservation business rules are tested.
- Boundary date cases are tested.
- All tests pass.

---

### SH-025: Write API controller tests

**Priority:** Medium | **Estimate:** 3 hours

**Tasks**

Use Moq to mock service dependencies.

Test the following scenarios:

- GET returns 200.
- Missing resource returns 404.
- POST returns 201.
- Invalid input returns 400.
- PUT returns expected status.
- Unauthorized requests are rejected.

Then:

- Verify that controllers call the correct service methods.
- Verify returned response types.
- Verify expected HTTP status codes.

**Acceptance criteria**

- Controller responses are tested.
- Service dependencies are mocked.
- Expected HTTP status codes are verified.
- All tests pass.

---

### SH-026: Write MVC controller and HttpClient service tests

**Priority:** Medium | **Estimate:** 3 hours

**Tasks**

Test the following scenarios:

- MVC catalogue calls item service.
- MVC controller passes returned items to the view.
- MVC handles empty results.
- MVC handles API failures.
- API service correctly deserializes JSON.
- API service handles unsuccessful HTTP responses.
- API service handles connection failures.

Then:

- Use a fake `HttpMessageHandler` or Moq to simulate HTTP responses.
- Ensure tests do not call the real localhost API.

**Acceptance criteria**

- MVC controllers have unit tests.
- `HttpClient` services have unit tests.
- API failures are handled correctly.
- All tests pass.

---

### SH-027: Create integration tests

**Priority:** Medium | **Estimate:** 4 hours

**Tasks**

- Configure `WebApplicationFactory`.
- Configure an isolated test database.
- Use SQLite in-memory for relational integration tests.
- Create HTTP tests for item endpoints.
- Test item creation.
- Test item retrieval.
- Test invalid input.
- Test missing resources.
- Test authentication.
- Test authorization.
- Test reservation workflows.
- Ensure test data is isolated.
- Clean up test data after tests.

**Acceptance criteria**

- Integration tests use a test environment.
- Production database is never used.
- Real HTTP requests are tested.
- Database interactions are verified.
- Tests clean up their test data.

---

## EPIC 11 – Error handling and security

### SH-028: Implement global exception handling

**Priority:** High | **Estimate:** 2 hours

**Tasks**

- Add global exception middleware.
- Return consistent `ProblemDetails` responses.
- Log exceptions.
- Avoid exposing stack traces in production.
- Handle API connection errors in MVC.
- Display friendly error pages.
- Handle unexpected HTTP responses.
- Handle missing resources consistently.

**Acceptance criteria**

- Exceptions are handled consistently.
- Errors are logged.
- Production responses do not expose sensitive details.
- MVC displays user-friendly error messages.

---

### SH-029: Implement security protections

**Priority:** Critical | **Estimate:** 3 hours

**Tasks**

- Use HTTPS.
- Validate incoming requests.
- Apply authentication to protected endpoints.
- Apply role-based authorization.
- Verify resource ownership.
- Add CSRF protection to MVC forms.
- Protect authentication cookies.
- Store secrets outside source control.
- Limit uploaded image size.
- Validate uploaded file types.
- Prevent mass assignment through request DTOs.
- Avoid exposing internal exception details.
- Implement basic request rate limiting where appropriate.

**Acceptance criteria**

- Anonymous users cannot perform protected operations.
- Users cannot edit other users' items.
- Users cannot approve unauthorized reservations.
- Production errors do not expose sensitive details.
- Authentication cookies use secure settings.
- Uploaded files are validated.

---

## EPIC 12 – User interface improvements

### SH-030: Improve application design

**Priority:** Medium | **Estimate:** 4 hours

**Tasks**

Use Bootstrap.

Create the following pages:

- Homepage.
- Item catalogue.
- Item details.
- Registration page.
- Login page.
- User dashboard.
- My items page.
- My reservations page.
- Incoming reservations page.
- Admin dashboard.

Then:

- Implement responsive navigation.
- Improve forms.
- Add validation messages.
- Add loading states.
- Add error states.
- Add empty states.
- Improve mobile layout.
- Improve desktop layout.
- Add consistent colours and typography.
- Add confirmation messages for important actions.

**Acceptance criteria**

- Responsive design works.
- Navigation works.
- Validation messages are visible.
- Loading and error states are handled.
- Empty states are displayed.
- Application is usable on mobile and desktop.

---

### SH-031: Implement item image management

**Priority:** Low | **Estimate:** 3 hours

**Tasks**

- Allow users to upload item images.
- Validate image file types.
- Restrict maximum image size.
- Generate unique image filenames.
- Store images in an appropriate storage location.
- Save image references in the database.
- Display item images in the catalogue.
- Display item images on the details page.
- Handle items without images.
- Prevent unauthorized image management.

**Acceptance criteria**

- Users can upload item images.
- Invalid file types are rejected.
- Image size restrictions work.
- Images display correctly.
- Image references are saved.
- Unauthorized users cannot modify another user's images.

---

## EPIC 13 – GitHub deployment (GitHub Actions CI/CD)

> **Note:** GitHub itself cannot run ASP.NET Core applications or SQL Server (GitHub Pages only serves static files). GitHub is used here as the source of truth and the deployment pipeline (GitHub Actions). Both applications (`ShareHub.API` and `ShareHub.MVC`) run on the MonsterASP free hosting plan, and the pipeline deploys to it automatically whenever code is pushed to `main`.

### SH-032: Create production hosting resources and GitHub secrets

**Priority:** Critical | **Estimate:** 2 hours

**Tasks**

- Create a free MonsterASP account.
- Verify the free plan allows two websites and a SQL Server database. If it does not, free up an existing slot or use a different host for one of the apps.
- Create a website for `ShareHub.API`.
- Create a separate website for `ShareHub.MVC`.
- Create a hosted SQL Server database.
- Configure HTTPS.
- Note the assigned website URLs.
- Create a database user.
- Retrieve the production database connection string.
- Retrieve the deployment credentials (Web Deploy or FTP) for both websites.
- Store the deployment credentials and the production connection string as GitHub repository secrets.

**Acceptance criteria**

- API website exists.
- MVC website exists.
- SQL Server database exists.
- Both websites support the required .NET runtime.
- HTTPS works.
- All credentials are stored as GitHub secrets and not in the repository.

---

### SH-033: Configure production database

**Priority:** Critical | **Estimate:** 2 hours

**Tasks**

- Configure production connection string securely.
- Ensure the API uses the production database.
- Keep the production database separate from development.
- Apply EF Core migrations through a controlled deployment process.
- Verify database connectivity.
- Verify required tables exist.
- Ensure credentials are not committed to Git.
- Avoid automatically running destructive migrations at startup.

**Acceptance criteria**

- API connects to production SQL Server.
- Database schema is up to date.
- Production database is separate from development.
- Database credentials are secure.

---

### SH-034: Deploy API with GitHub Actions

**Priority:** Critical | **Estimate:** 3 hours

**Tasks**

- Create a workflow in `.github/workflows/` for `ShareHub.API`.
- Trigger the workflow on pushes to `main` (and on pull requests for build and test only).
- Restore, build and run all tests (`dotnet test`) before deploying.
- Publish the API with `dotnet publish -c Release`.
- Deploy the published output to the MonsterASP API website from the workflow (Web Deploy or FTP) using the credentials stored in GitHub secrets.
- Deploy only if build and tests succeed.
- Open the API website.
- Test Swagger where enabled.
- Test API endpoints.
- Verify database connectivity.

**Acceptance criteria**

- Workflow runs successfully.
- Deployment is blocked if tests fail.
- API starts successfully.
- Swagger is available where configured.
- API can access production SQL Server.
- API endpoints return expected responses.

---

### SH-035: Deploy MVC with GitHub Actions

**Priority:** Critical | **Estimate:** 3 hours

**Tasks**

- Deploy the API first, so that the MVC application has a running API to call.
- Configure production API `BaseUrl`.
- Verify API address uses HTTPS.
- Create a workflow in `.github/workflows/` for `ShareHub.MVC`.
- Restore, build and run tests before deploying.
- Publish MVC with `dotnet publish -c Release`.
- Deploy the published output to the MonsterASP MVC website from the workflow (Web Deploy or FTP) using the credentials stored in GitHub secrets.
- Open the hosted MVC website.
- Test catalogue.
- Test item details.
- Test registration and login.
- Test reservation requests.
- Verify there are no localhost references in production configuration.

**Acceptance criteria**

- Workflow runs successfully.
- MVC starts successfully.
- MVC communicates with hosted API.
- API requests work over HTTPS.
- No localhost references remain in production configuration.
- Full application workflow works online.

---

### SH-036: Perform production testing

**Priority:** High | **Estimate:** 2 hours

**Tasks**

- Test user registration.
- Test user login.
- Test item creation.
- Test item editing.
- Test item search.
- Test reservation requests.
- Test reservation approval.
- Test reservation rejection.
- Test reservation cancellation.
- Test authorization.
- Test database persistence.
- Test error handling.
- Test the application on mobile and desktop.
- Check application logs.
- Check for broken links and missing assets.

**Acceptance criteria**

- All main workflows work online.
- No critical errors remain.
- Database operations work correctly.
- Authentication works.
- Authorization works.
- MVC and API communicate successfully.

---

## EPIC 14 – Documentation and GitHub

### SH-037: Prepare GitHub repository

**Priority:** High | **Estimate:** 2 hours

**Tasks**

- Create GitHub repository.
- Configure repository as public.
- Add `.gitignore`.
- Exclude `bin` and `obj` directories.
- Exclude local secrets.
- Exclude local databases.
- Exclude sensitive configuration files.
- Add `README.md`.
- Commit code regularly.
- Use descriptive commit messages.
- Create feature branches.
- Use pull requests to merge completed features.

**Suggested commit messages**

- Initial solution setup.
- Add EF Core database context.
- Implement item CRUD endpoints.
- Add HttpClient integration.
- Implement reservation service.
- Add NUnit tests for reservations.
- Add Identity authentication.
- Prepare production deployment.

**Acceptance criteria**

- Repository is public.
- No secrets are committed.
- Source code is organized.
- Commits are meaningful.
- Feature branches are used.

---

### SH-038: Write project documentation

**Priority:** High | **Estimate:** 2 hours

**Tasks**

Create `README.md` containing:

- Project overview.
- Technologies used.
- Architecture diagram.
- Solution structure.
- Database diagram.
- API endpoint documentation.
- Local development setup.
- Connection string configuration.
- How to run the tests.
- Production deployment overview.
- Screenshots.

Then:

- Create `docs/architecture.md`.
- Create `docs/database-design.md`.
- Document important business rules.
- Document reservation status transitions.
- Document authentication and authorization.
- Document how to configure the development environment.

**Acceptance criteria**

- Another developer can clone the project.
- Developer can configure local database.
- Developer can run both applications.
- Developer can execute tests.
- API endpoints are documented.
- Screenshots demonstrate completed application.
- Documentation is accurate and up to date.

---

### SH-039: Final GitHub repository cleanup

**Priority:** Medium | **Estimate:** 1 hour

**Tasks**

- Review repository structure.
- Remove unused files.
- Remove temporary files.
- Check for accidentally committed secrets.
- Check `.gitignore` rules.
- Ensure all migrations are committed.
- Ensure README contains setup instructions.
- Add application screenshots.
- Add project description to GitHub.
- Add relevant repository topics.
- Verify repository can be cloned successfully.

**Acceptance criteria**

- Repository is clean and organized.
- No sensitive files are exposed.
- Project can be cloned.
- Project documentation is complete.
- Repository is ready to be included in a CV.

---

## Final Definition of Done

The project is complete when:

- API and MVC are separate projects.
- Both run simultaneously in Visual Studio.
- MVC communicates with API through `HttpClient`.
- API uses EF Core and SQL Server.
- Development and production environments are separated.
- Users can register and log in.
- Users can create and manage items.
- Users can browse and search items.
- Users can request reservations.
- Owners can approve and reject requests.
- Overlapping approved reservations are prevented.
- Authorization protects private operations.
- Users can leave reviews.
- Users can manage favourites.
- Administrators can manage categories.
- NUnit tests cover important business rules.
- Integration tests verify HTTP endpoints.
- Application is deployed to MonsterASP.
- README and screenshots are available on GitHub.

---

## Recommended Development Order

1. Solution setup and architecture.
2. Database and Entity Framework Core.
3. API architecture.
4. Item management API.
5. MVC and API communication.
6. Authentication and authorization.
7. Reservation system.
8. NUnit unit and integration testing.
9. Error handling and security.
10. User interface improvements.
11. Reviews and favourites.
12. Administration and dashboard.
13. GitHub deployment (GitHub Actions CI/CD).
14. Documentation and GitHub cleanup.

---

## Development Rules

- Complete one story at a time.
- Do not move a ticket to DONE until acceptance criteria are met.
- Write unit tests alongside business logic.
- Use meaningful Git commits.
- Keep development and production databases separate.
- Never commit passwords or secrets.
- Test API endpoints independently through Swagger.
- Test MVC communication through `HttpClient`.
- Keep controllers thin.
- Keep business logic inside services.
- Keep database access inside repositories.
- Prefer readable, maintainable code over unnecessary complexity.
- Document important architectural decisions.
- Do not introduce unnecessary design patterns or dependencies.

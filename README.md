Welcome to HahnProject! This repository hosts a comprehensive full-stack application designed with clean architecture principles, ensuring scalability, maintainability, and ease of development. The project encompasses both frontend and backend components, each meticulously structured to facilitate efficient development workflows and seamless integration.



📑 Table of Contents
🌟 Overview
🛠️ Technologies Used
Backend
Frontend
📂 Project Structure
Backend
Frontend
🚀 Getting Started
Prerequisites
Backend Setup
Frontend Setup
📚 Documentation
🧪 Testing
🤝 Contributing
📝 License
📞 Contact
🌟 Overview
HahnProject is a full-stack application built with a focus on clean architecture, promoting separation of concerns and enhancing code maintainability. The project is divided into distinct layers for the backend and frontend, each responsible for specific functionalities, ensuring a robust and scalable system.

Project Goals
Clean Architecture Implementation: Demonstrate the principles of clean architecture in a real-world application.
Seamless Frontend-Backend Integration: Ensure efficient communication and data flow between frontend and backend.
Scalability and Maintainability: Build a system that can grow and adapt with evolving requirements.
Comprehensive Documentation: Provide clear guidelines and documentation for easy onboarding and contribution.
🛠️ Technologies Used
Backend
.NET 6: Modern, cross-platform framework for building high-performance applications.
ASP.NET Core Web API: Framework for creating RESTful APIs.
Entity Framework Core: Object-Relational Mapping (ORM) tool for database interactions.
PostgreSQL: Reliable and robust open-source relational database system.
JWT (JSON Web Tokens): Secure token-based authentication.
Swagger: Interactive API documentation and testing tool.
Mixpanel: Advanced analytics platform for tracking user interactions and behaviors.
Frontend
Vue.js 3: Progressive JavaScript framework for building user interfaces.
TypeScript: Superset of JavaScript adding static typing for enhanced code quality.
Vite: Fast and lean development build tool for frontend projects.
Vue Router: Official router for Vue.js for managing application routes.
Vuex: State management pattern + library for Vue.js applications.
Axios: Promise-based HTTP client for making API requests.
Vue Toastification: Elegant toast notifications for Vue.js.
Other Tools
Docker (Optional): Containerization platform for consistent development and deployment environments.
Git: Version control system for tracking changes and collaboration.
ESLint & Prettier: Tools for maintaining code quality and consistent formatting.
📂 Project Structure
The HahnProject is organized following the principles of clean architecture, ensuring a clear separation of concerns between different parts of the application. Below is an overview of the project's directory structure:
<br/>
<br/>Backend Folder Structure<br/><br/>
HahnProject.sln<br/>
└── src<br/>
    ├── Hahn.Domain<br/>
    │   ├── Entities<br/>
    │   ├── ValueObjects<br/>
    │   ├── Events<br/>
    │   └── ...<br/>
    ├── Hahn.Application<br/>
    │   ├── Interfaces<br/>
    │   ├── DTOs<br/>
    │   ├── Features (Commands, Queries)<br/>
    │   ├── Behaviors<br/>
    │   ├── Mappings<br/>
    │   └── ...<br/>
    ├── Hahn.Infrastructure<br/>
    │   ├── Data<br/>
    │   │   ├── HahnDbContext.cs<br/>
    │   │   ├── Migrations<br/>
    │   ├── Repositories<br/>
    │   ├── ExternalServices<br/>
    │   └── ...<br/>
    ├── Hahn.Jobs<br/>
    │   ├── IFoodRecipeUpsertJob.cs<br/>
    │   ├── FoodRecipeUpsertJob.cs<br/>
    │   └── ...<br/>
    ├── Hahn.WebAPI<br/>
    │   ├── Controllers<br/>
    │   ├── Program.cs<br/>
    │   ├── appsettings.json<br/>
    │   └── ...<br/>
    ├── Hahn.WorkerService<br/>
    │   ├── Program.cs<br/>
    │   ├── Worker.cs<br/>
    │   ├── appsettings.json<br/>
    │   └── ...<br/>
    └── Hahn.Frontend<br/>
        ├── package.json<br/>
        ├── tsconfig.json<br/>
        ├── vite.config.js<br/>
        ├── public<br/>
        └── src<br/>
            ├── main.ts<br/>
            ├── App.vue<br/>
            ├── components<br/>
            │   └── RecipesGrid.vue<br/>
            └── ...<br/>

Frontend Folder Structure<br/>


<br/>Hahn.Frontend<br/>
├── package.json<br/>
├── tsconfig.json<br/>
├── vite.config.js<br/>
├── public<br/>
└── src<br/>
    ├── main.ts<br/>
    ├── App.vue<br/>
    ├── components<br/>
    │   └── RecipesGrid.vue<br/>
    ├── views<br/>
    │   └── Home.vue<br/>
    ├── router<br/>
    │   └── index.ts<br/>
    ├── store<br/>
    │   └── index.ts<br/>
    ├── services<br/>
    │   └── axiosInstance.ts<br/>
    ├── assets<br/>
    │   └── logo.png<br/>
    └── ...<br/>
Detailed Description<br/>
1. Hahn.Domain
Entities: Core business objects representing data structures.
ValueObjects: Immutable objects that represent values without an identity.
Events: Domain events that trigger actions within the system.
...: Additional domain-related classes and interfaces.
2. Hahn.Application
Interfaces: Contracts for services and repositories.
DTOs (Data Transfer Objects): Objects used to transfer data between layers.
Features (Commands, Queries): Implementation of application logic using CQRS (Command Query Responsibility Segregation) pattern.
Behaviors: Pipeline behaviors for handling cross-cutting concerns.
Mappings: Configuration for object-object mapping, often using AutoMapper.
...: Additional application layer components.
3. Hahn.Infrastructure
Data:
HahnDbContext.cs: EF Core database context.
Migrations: Database migration files.
Repositories: Concrete implementations of repository interfaces for data access.
ExternalServices: Integrations with third-party services.
...: Additional infrastructure-related components.
4. Hahn.Jobs
IFoodRecipeUpsertJob.cs: Interface defining the contract for the job.
FoodRecipeUpsertJob.cs: Implementation of the job handling food recipe upserts.
...: Additional background jobs and related classes.
5. Hahn.WebAPI
Controllers: API controllers handling HTTP requests.
Program.cs: Entry point of the Web API application.
appsettings.json: Configuration settings.
...: Additional Web API components and configurations.
6. Hahn.WorkerService
Program.cs: Entry point of the Worker Service.
Worker.cs: Implementation of the background worker.
appsettings.json: Configuration settings.
...: Additional worker service components.
7. Hahn.Frontend
package.json: Project metadata and dependencies.
tsconfig.json: TypeScript configuration.
vite.config.js: Vite build tool configuration.
public: Static assets accessible at the root of the application.
src:
main.ts: Entry point of the Vue.js application.
App.vue: Root Vue component.
components: Reusable Vue components.
RecipesGrid.vue: Component displaying a grid of recipes.
views: Vue components representing different pages.
Home.vue: Home page view.
router: Vue Router configuration.
index.ts: Route definitions.
store: Vuex store configuration.
index.ts: Store modules and state management.
services:
axiosInstance.ts: Configured Axios instance for API requests.
assets: Images, fonts, and other static assets.
logo.png: Project logo.
...: Additional frontend components and configurations.
🚀 Getting Started
Follow these instructions to set up and run HahnProject on your local machine.

Prerequisites
Ensure you have the following tools and software installed:

.NET 8 SDK: Download Here
Node.js (v14 or later): Download Here
Yarn or npm: Package managers for JavaScript.
Yarn: Installation Guide
npm: Comes bundled with Node.js.
PostgreSQL: Download Here (if using the database)
Docker (Optional): Download Here for containerization.
Git: Download Here for version control.
Backend Setup
Clone the Repository<br/>

git clone https://github.com/ivanbarros/HahnProject.git<br/>
cd HahnProject<br/>
Navigate to the Backend Directory<br/>

cd src/Hahn.WebAPI<br/>
Restore Dependencies and Build the Project<br/>

dotnet restore
dotnet build
Configure the Database

Update Migrations
dotnet ef database update
Configure Connection Strings:
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=HahnDb;Username=your-username;Password=your-password"
  },
  ...
}<br/><br/>
Run the Backend API

dotnet run
The API will be accessible.

Frontend Setup
Open a New Terminal Window and Navigate to the Frontend Directory

cd src/Hahn.Frontend
Install Dependencies

Using Yarn:

yarn install<br/>
Or using npm:<br/>

npm install
Configure Environment Variables


Start the Frontend Development Server

Using Yarn:

yarn dev<br/>
Or using npm:<br/>

npm run dev
The frontend application will be accessible.

📚 Documentation
Comprehensive documentation is provided to assist with understanding the architecture, components, and usage of the project.

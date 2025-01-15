# HahnProject

HahnProject.sln
└── src
    ├── Hahn.Domain
    │   ├── Entities
    │   ├── ValueObjects
    │   ├── Events
    │   └── ...
    ├── Hahn.Application
    │   ├── Interfaces
    │   ├── DTOs
    │   ├── Features (Commands, Queries)
    │   ├── Behaviors
    │   ├── Mappings
    │   └── ...
    ├── Hahn.Infrastructure
    │   ├── Data
    │   │   ├── HahnDbContext.cs
    │   │   ├── Migrations
    │   ├── Repositories
    │   ├── ExternalServices
    │   └── ...
    ├── Hahn.Jobs
    │   ├── IFoodRecipeUpsertJob.cs
    │   ├── FoodRecipeUpsertJob.cs
    │   └── ...
    ├── Hahn.WebAPI
    │   ├── Controllers
    │   ├── Program.cs
    │   ├── appsettings.json
    │   └── ...
    ├── Hahn.WorkerService
    │   ├── Program.cs
    │   ├── Worker.cs
    │   ├── appsettings.json
    │   └── ...
    └── Hahn.Frontend
        ├── package.json
        ├── tsconfig.json
        ├── vite.config.js
        ├── public
        └── src
            ├── main.ts
            ├── App.vue
            ├── components
            │   └── RecipesGrid.vue
            └── ...

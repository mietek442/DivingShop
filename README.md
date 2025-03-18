# DivingShop

# Robienie Migracji, Migrations

```docker command
dotnet ef migrations add "InitialMigration"  --output-dir Infrastructure/Persistence/Migrations
```

🥴
-Upewnij się, że zmieniasz connection string z serwerem podczas tworzenia migracji, a po wykonaniu migracji zmień connection string połączenia domyślnego z powrotem.

-Make sure to change the server connection when you make migration string in your, after migrations you change defaultconnection string back `appsettings.json` file from:

```json
"DefaultConnection": "Server=restauracja.database;Database=restauracja;User Id=restauracja;Password=restauracja;"
```

to:

```json
"DefaultConnection": "Server=localhost;Database=restauracja;User Id=restauracja;Password=restauracja;"
```

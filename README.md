# DivingShop
<a href="https://wakatime.com/badge/user/ceb09352-f08e-4020-bcd8-6ecb02d0d589/project/3d9d15eb-fe24-4f3b-b1da-47044ad39c59"><img src="https://wakatime.com/badge/user/ceb09352-f08e-4020-bcd8-6ecb02d0d589/project/3d9d15eb-fe24-4f3b-b1da-47044ad39c59.svg" alt="wakatime"></a>
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

DivingShop to projekt API sklepu internetowego stworzonego w technologii .NET, zaprojektowany dla pasjonatów nurkowania. Aplikacja korzysta z Entity Framework do zarządzania bazą danych oraz z implementacji systemu uwierzytelniania (Entity Framework Authentication), który obsługuje logowanie i rejestrację użytkowników.

## Funkcje projektu

- **Logowanie i rejestracja** – bezpieczny dostęp dla użytkowników.
- **Przegląd produktów** – zwracanie informacji o sprzęcie wraz ze zdjęciami.
- **Koszyk** – funkcje dodawania i modyfikacji zamówienia.
- **Płatności** – realizacja transakcji online.

Projekt stanowi solidną bazę dla e-commerce w branży nurkowej, zapewniając wygodę i bezpieczeństwo dla użytkowników.

## Wykorzystane technologie

- **Entity Framework Core** – ORM (Object-Relational Mapper), który upraszcza operacje na bazie danych, umożliwiając mapowanie obiektów na tabele oraz zarządzanie relacjami między nimi.
- **Entity Framework Authentication** – system uwierzytelniania i autoryzacji użytkowników, który zabezpiecza dane oraz umożliwia bezpieczne logowanie i rejestrację.


DivingShop oferuje nowoczesne i bezpieczne rozwiązanie dla miłośników nurkowania, umożliwiając im łatwe zakupy online.


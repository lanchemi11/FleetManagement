# Sistem za upravljanje voznim parkom (Fleet Management System)

Ovaj projekat predstavlja razvoj modularne WPF aplikacije uz primenu Clean i Heksagonalne (Ports and Adapters) arhitekture u obliku modularnog monolita. Projekat je rađen kao ispitni rad iz predmeta **Arhitektura softverskih sistema**.

## Tehnologije i alati
- **UI:** WPF (.NET 6+), XAML
- **Arhitektura:** Modularni monolit, Clean Architecture, Hexagonal Architecture
- **Dependency Injection:** Microsoft.Extensions.DependencyInjection
- **Perzistencija:** JSON fajlovi, In-Memory
- **Testiranje:** xUnit, Moq (Mocking)
- **CI/CD:** GitHub Actions

## Struktura projekta (Modularni monolit)
Projekat se sastoji od **5 funkcionalnih modula** koji su labavo povezani i komuniciraju isključivo asinhrono preko Event Bus-a:

1. **Vehicles (Vozila):** Upravljanje vozilima i njihovim stanjima.
2. **Drivers (Vozači):** Evidencija vozača i njihovih licenci.
3. **Trips (Vožnje):** Složeni Use Case-ovi za pokretanje i završetak vožnji (povezuje vozila i vozače).
4. **Notifications (Obaveštenja):** Sluša događaj `TripCompletedEvent` i upisuje sistemske logove.
5. **Maintenance (Održavanje):** Evidencija servisa i privremeno povlačenje vozila iz opticaja.

Pored funkcionalnih modula, sistem sadrži:
- **SharedKernel:** Zajednički tipovi, bazični entiteti (`Entity`) i `InMemoryEventBus`.
- **WPF App (Shell):** Pokretački projekat koji kroz DI povezuje sve module u jednu celinu.

## Heksagonalni pristup (Ports and Adapters)
Svaki modul je strogo podeljen na slojeve unutar foldera:
- **Domain:** Čisti domenski entiteti i biznis pravila.
- **Application (Ports):** Use Case-ovi i interfejsi repozitorijuma.
- **Infrastructure (Adapters):** Konkretne implementacije repozitorijuma (JSON i InMemory verzije).

## Kako pokrenuti projekat
Pozicionirajte se u koren projekta i pokrenite:
```bash
dotnet run --project src/Presentation/FleetManagement.WPF/

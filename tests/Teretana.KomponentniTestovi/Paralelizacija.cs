// Klase se izvršavaju paralelno, a KomponentniTest paralelizuje i metode unutar klase.
// Nova instanca klase po testu garantuje da testovi ne dele polja (aplikaciju, klijenta, bazu).
[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]

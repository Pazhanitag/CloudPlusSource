namespace CloudPlus.Database.Authentication
{
    internal interface IMigrationSeed<in TContext>
    {
        void Seed(TContext context);
    }
}

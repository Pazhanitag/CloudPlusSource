namespace CloudPlus.Database
{
    internal interface IMigrationSeed<in TContext>
    {
        void Seed(TContext context);
    }
}
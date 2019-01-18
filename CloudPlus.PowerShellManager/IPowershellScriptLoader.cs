namespace CloudPlus.PowerShell
{
    public interface IPowershellScriptLoader
    {
        string LoadScript(string scriptName);
    }
}
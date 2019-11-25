namespace TCPlayer.Engine.Internals
{
    internal class ExceptionFactory
    {
        public static EngineException Create(int errorCode, string message)
        {
            throw new EngineException($"{message}, code: {errorCode}");
        }
    }
}

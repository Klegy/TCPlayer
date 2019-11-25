namespace TCPlayer.Engine.Internals
{
    internal class ExceptionFactory
    {
        public EngineException Create(int errorCode, string message)
        {
            return new EngineException($"{message}, code: {errorCode}");
        }
    }
}

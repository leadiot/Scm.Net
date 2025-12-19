namespace Com.Scm.Terminal
{
    public interface ITerminalHolder
    {
        public ScmTerminalToken GetTerminalByToken(string token);

        public void Remote(string token);

        public void Clear();
    }
}

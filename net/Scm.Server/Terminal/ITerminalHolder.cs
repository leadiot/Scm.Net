namespace Com.Scm.Terminal
{
    public interface ITerminalHolder
    {
        public ScmTerminalToken GetTerminal(long id);

        public void Remote(long id);

        public void Clear();
    }
}

namespace Bot
{
    public abstract class BotField_Template
    {
        protected BotAgent m_operator { get; private set; }

        public BotField_Template(BotAgent bot_)
        {
            m_operator = bot_;
        }
    }
}
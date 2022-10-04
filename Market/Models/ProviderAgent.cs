namespace Market.Models;

public class ProviderAgent
{
    public ProviderAgent(User agent, Provider provider)
    {
        if (agent.Type != UserType.Agent)
            throw new ArgumentException($"Given user is not agent");

        Agent = agent;
        Provider = provider;
    }

    public User Agent { get; private set; }
    public Provider Provider { get; private set; }
}

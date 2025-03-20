using Microsoft.Azure.Functions.Worker;

namespace Electrolux.BFF.ComparePage.Functions
{
    public class Warmup
    {
        [Function(nameof(Warmup))]
        public void Run([WarmupTrigger] object warmupContext, FunctionContext context)
        {
           // TODO The warmup trigger isn't available to apps running on the Consumption plan.
        }
    }
}
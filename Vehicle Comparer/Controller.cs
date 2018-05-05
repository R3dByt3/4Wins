using DiMappings;
using Ninject;

namespace GUI
{
    public static class Controller
    {
        public static StandardKernel StandardKernel = new StandardKernel(new Aggregator().Mappings);
    }
}

using System.Linq;
using System.Collections.Generic;
using FDR.Tools.Library;

namespace FDR
{
    public sealed class AppConfig
    {
        public Dictionary<string, ImportConfig> ImportConfigs { get; } = new Dictionary<string, ImportConfig>();

        public Dictionary<string, RenameConfig> RenameConfigs { get; } = new Dictionary<string, RenameConfig>();

        public Dictionary<string, ResizeConfig> ResizeConfigs { get; } = new Dictionary<string, ResizeConfig>();

        public Dictionary<string, BatchRenameConfig> BatchRenameConfigs { get; } = new Dictionary<string, BatchRenameConfig>();

        public Dictionary<string, BatchResizeConfig> BatchResizeConfigs { get; } = new Dictionary<string, BatchResizeConfig>();

        public void Validate()
        {
            ImportConfigs?.ToList().ForEach(ic => ic.Value.Validate());
            RenameConfigs?.ToList().ForEach(rnc => rnc.Value.Validate());
            ResizeConfigs?.ToList().ForEach(rsc => rsc.Value.Validate());
            BatchRenameConfigs?.ToList().ForEach(brnc => brnc.Value.Validate());
            BatchResizeConfigs?.ToList().ForEach(brsc => brsc.Value.Validate());
        }
    }
}

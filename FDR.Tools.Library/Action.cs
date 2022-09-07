using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FDR.Tools.Library
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ActionType
    {
        rename,
        move,
        resize,
        hash,
        rehash
    }

    public class ActionBase
    {
        protected internal ActionBase(ActionType actionType) { ActionType = actionType; }

        public ActionType ActionType { get; }
    }

    public class RenameAction : ActionBase
    {
        public RenameAction() : base(ActionType.rename) { }
    }

    public class MoveAction : ActionBase
    {
        public MoveAction() : base(ActionType.move) { }
    }

    public class ResizeAction : ActionBase
    {
        public ResizeAction() : base(ActionType.resize) { }
    }

    public class HashAction : ActionBase
    {
        public HashAction() : base(ActionType.hash) { }
    }

    public class RehashAction : ActionBase
    {
        public RehashAction() : base(ActionType.rehash) { }
    }
}

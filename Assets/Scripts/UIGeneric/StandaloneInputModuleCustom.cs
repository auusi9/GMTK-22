using UnityEngine.EventSystems;

namespace UIGeneric
{
    public class StandaloneInputModuleCustom : StandaloneInputModule {
 
        public PointerEventData GetLastPointerEventDataPublic (int id)
        {
            return GetLastPointerEventData(id);
        }
    }
}
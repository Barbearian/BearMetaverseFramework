
using UnityEngine;

namespace Bear
{
    public static class EquipmentFactory 
    {
        public static INode MakeEquipment(this EquipmentData data) {
            var rs = new GameObject(data.equipmentName).AddComponent<NodeView>();
            
            return rs;
        } 
    }
}
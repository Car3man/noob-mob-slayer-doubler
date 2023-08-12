using UnityEngine;

namespace Game.Mobs
{
    public class MobShadow : MonoBehaviour
    {
        private void Update()
        {
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);       
        }
    }
}

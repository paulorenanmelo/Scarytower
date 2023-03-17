using Unity.Entities;

// Authoring MonoBehaviours are regular GameObject components.
// They constitute the inputs for the baking systems which generates ECS data.
class STPlayerAuthoring : UnityEngine.MonoBehaviour
{
    // Bakers convert authoring MonoBehaviours into entities and components.
    // (Nesting a baker in its associated Authoring component is not necessary but is a common convention.)
    class STPlayerBaker : Baker<STPlayerAuthoring>
    {
        public override void Bake(STPlayerAuthoring authoring)
        {
            AddComponent<STPlayer>();
        }
    }
}

// An ECS component.
// An empty component is called a "tag component".
struct STPlayer : IComponentData
{
}
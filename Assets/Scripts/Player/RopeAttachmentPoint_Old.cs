// using System.Collections;
// using System.Collections.Generic;
// using Cysharp.Threading.Tasks;
// using UnityEngine; 
// using Ltg8.Inventory;
//
// namespace Ltg8.Player
// {
//     public class RopeAttachmentPoint : ProximityInteractable
//     {
//         private bool isDeployed = false;
//         public ItemData ropeData;
//         public Mesh attachedMesh;
//         public Mesh unattachedMesh;
//         private MeshFilter meshFilter;
//
//
//         // Start is called before the first frame update
//         void Start()
//         {
//             meshFilter = GetComponent<MeshFilter>();
//             if(isDeployed){
//                 promptText = "Collect Rope";
//                 meshFilter.mesh = attachedMesh;
//             }else{
//                 meshFilter.mesh = unattachedMesh;
//             }
//         }
//
//         public void deployRope(){
//             if(!isDeployed){
//                 Debug.Log("Rope isn't deployed. Deploying rope!");
//                 promptText = "Collect Rope";
//                 isDeployed = !isDeployed;
//                 meshFilter.mesh = attachedMesh;
//             }
//         }
//
//         public void collectRope(){
//             if(isDeployed)
//             {
//                 Debug.Log("Rope is already deployed. Collecting rope!");
//                 promptText = "";
//                 InventoryUtil.AddItem(ropeData).Forget();
//                 isDeployed = !isDeployed;
//                 meshFilter.mesh = unattachedMesh;
//             }
//         }
//     }
// }
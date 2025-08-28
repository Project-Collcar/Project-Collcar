using Fusion;
using UnityEngine;
using Unity.Cinemachine;
[RequireComponent(typeof(NetworkObject))]
public class SpeederCamera : NetworkBehaviour
{
    // 씬에 있는 CinemachineVirtualCamera를 담아둘 변수
    private CinemachineCamera virtualCamera;

    public override void Spawned()
    {
        // HasInputAuthority는 이 오브젝트가 '내 컴퓨터에서 직접 조종하는' 것인지를 확인함.
        // 다른 플레이어의 자동차에 내 카메라가 붙는 것을 막을 수 있음.
        if (HasInputAuthority)
        {
            // "SpeederCamera" 태그를 가진 게임 오브젝트를 찾습니다.
            GameObject cameraObject = GameObject.FindWithTag("SpeederCamera");
            
            // 카메라 오브젝트를 찾았는지, 그리고 그 안에 CinemachineCamera 컴포넌트가 있는지 확인합니다.
            if(cameraObject != null)
            {
            virtualCamera = cameraObject.GetComponent<CinemachineCamera>();
            }
            
            // virtualCamera 변수에 카메라가 성공적으로 할당되었는지 최종 확인합니다.
            if (virtualCamera != null)
            {
                // 찾은 카메라의 Follow와 LookAt 타겟을 '나 자신(이 스크립트가 붙어있는 오브젝트)'으로 설정합니다.
                virtualCamera.Follow = this.transform;
                
                Debug.Log("Cinemachine camera target set using Tag successfully!");
            }
            else
            {
                // 문제를 쉽게 파악할 수 있도록 에러 로그를 남깁니다.
                Debug.LogError("PlayerCameraSetup: Could not find GameObject with tag 'Speeder' or it's missing the CinemachineCamera component!");
            }
        }
        
    }
}

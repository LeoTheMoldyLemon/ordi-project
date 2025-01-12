using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public void Play(AudioClip clip, float volume)
    {
        AudioManager.Instance.PlaySFX(clip, volume, transform.position);
    }
    public void PlayRandom(AudioClip[] clips, float volume)
    {
        AudioManager.Instance.PlaySFX(clips[Random.Range(0, clips.Length)], volume, transform.position);
    }

}


org.springframework.security.authorization.AuthorizationDeniedException: Access Denied
\tat org.springframework.security.authorization.method.ThrowingMethodAuthorizationDeniedHandler.handleDeniedInvocation(ThrowingMethodAuthorizationDeniedHandler.java:38)
\tat org.springframework.security.authorization.method.AuthorizationManagerBeforeMethodInterceptor.handle(AuthorizationManagerBeforeMethodInterceptor.java:290)
\tat org.springframework.security.authorization.method.AuthorizationManagerBeforeMethodInter…at org.apache.tomcat.util.net.NioEndpoint$SocketProcessor.doRun(NioEndpoint.java:1741)
\tat org.apache.tomcat.util.net.SocketProcessorBase.run(SocketProcessorBase.java:52)
\tat org.apache.tomcat.util.threads.ThreadPoolExecutor.runWorker(ThreadPoolExecutor.java:1190)
\tat org.apache.tomcat.util.threads.ThreadPoolExecutor$Worker.run(ThreadPoolExecutor.java:659)
\tat org.apache.tomcat.util.threads.TaskThread$WrappingRunnable.run(TaskThread.java:63)
\tat java.base / java.lang.Thread.run(Thread.java:1575)
"
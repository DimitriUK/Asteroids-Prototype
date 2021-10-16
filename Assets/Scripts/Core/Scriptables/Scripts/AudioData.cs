using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Data", menuName = "Audio Data/New Audio Data", order = 51)]
public class AudioData : ScriptableObject
{
    public AudioClip[] ClipsToPlay;
}

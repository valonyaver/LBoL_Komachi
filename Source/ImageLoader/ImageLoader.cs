using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KomachiMod.ImageLoader
{
    public sealed class KomachiImageLoader
    {
        public static string file_extension = ".png";
        public static PlayerImages LoadPlayerImages(string name)
        {
            PlayerImages sprites = new PlayerImages();
            sprites.AutoLoad(name, (s) => ResourceLoader.LoadSprite(s, BepinexPlugin.directorySource, ppu: 100, 1, FilterMode.Bilinear, generateMipMaps: true), (s) => ResourceLoader.LoadSpriteAsync(s, BepinexPlugin.directorySource));
            return sprites;
        }
        
        public static CardImages LoadCardImages(CardTemplate cardTemplate)
        {
            var imgs = new CardImages(BepinexPlugin.embeddedSource);
            imgs.AutoLoad(cardTemplate, extension: file_extension);
            return imgs;
        }

        public static ExhibitSprites LoadExhibitSprite(ExhibitTemplate exhibit)
        {
            var exhibitSprites = new ExhibitSprites();
            exhibitSprites.main = ResourceLoader.LoadSprite(exhibit.GetId() + file_extension, BepinexPlugin.embeddedSource);;
            return exhibitSprites;
        }

        public static IntentionImages LoadIntentionLoader(IntentionTemplate intention)
        {
            var imgs = new IntentionImages();
            imgs.main = LoadSprite(intention.GetId());
            return imgs;
        }

        public static GameObject effectParent;
        public static Dictionary<string, GameObject> effectObjects = new Dictionary<string, GameObject>();
        public static GameObject LoadEffectGameObject(string imageName)
        {
            if (effectParent == null)
            {
                GameObject parent = new GameObject("EffectParent");
                effectParent = parent;
                effectParent.transform.position = new Vector3(-10, 0, 0);
            }

            if (effectObjects.TryGetValue(imageName, out var effectGOExists))
            {
                return effectGOExists;
            }
            string imagePath = imageName + file_extension;

            Sprite effectSprite = ResourceLoader.LoadSprite(imagePath, BepinexPlugin.embeddedSource, ppu: 256);

            Debug.Log($"Trying to load {imageName}");

            if (effectSprite == null)
            {
                // Handle error: log a warning if the sprite couldn't be loaded
                Debug.LogWarning($"Failed to load effect sprite at path: {imagePath}");
                return new GameObject(imageName + "_Failed");
            }

            
            GameObject effectGO = new GameObject(imageName + "_Effect");
            effectGO.transform.parent = effectParent.transform;
            effectGO.AddComponent<EffectHider>();

            SpriteRenderer renderer = effectGO.AddComponent<SpriteRenderer>();
            renderer.sprite = effectSprite;

            ParticleSystem ps = effectGO.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.playOnAwake = false;
            main.stopAction = ParticleSystemStopAction.Disable; 

            var emission = ps.emission;
            emission.enabled = false; 

            effectGO.AddComponent<ParticleSystemRenderer>();
            return effectGO;
        }

        public class EffectHider :MonoBehaviour
        {
            void Start()
            {
                if (!name.Contains("Clone")){
                    StartCoroutine(HideAfterFrame());
                }
            }

            IEnumerator HideAfterFrame()
            {
                yield return null;
                transform.localPosition = new Vector3(0,0,0);
            }
        }

        public static Sprite LoadUltLoader(UltimateSkillTemplate ult)
        {
            return LoadSprite(ult.GetId());
        }

        public static Sprite LoadStatusEffectLoader(StatusEffectTemplate status)
        {
            return LoadSprite(status.GetId());
        }

        public static Sprite LoadSprite(IdContainer ID)
        {
            return ResourceLoader.LoadSprite(ID + file_extension, BepinexPlugin.embeddedSource);
        }
    }
}
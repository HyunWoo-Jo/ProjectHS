using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System;

namespace Data
{

    public enum MapTema {
        Spring,
        Winter,
        Desert,
    }

    public enum TileType {
        Ground = 0,
        Road_Vertical,
        Road_Horizontal,
        Road_Cross,
        Road_HorizontalUp,
        Road_VerticalLeft,
        Road_VerticalRight,
        Road_HorizontalDown,
        Road_RightDown,
        Road_LeftUp,
        Road_RightUp,
        Road_LeftDown,
    }

    public struct MapData
    {
        public Vector3 position;
        public TileType type;
        public int orderBy;
    }

    /// <summary>
    /// TileType과 Sprite를 매칭
    /// </summary>
    public class TileSpriteMapper {

        private Dictionary<TileType, Sprite> _spriteDic;

        public IReadOnlyDictionary<TileType, Sprite> GetSpriteDictionary() { // readonly
            if (_spriteDic == null) {
                return null;
            }
            return _spriteDic;
        }

        public bool IsLoad { get; private set; }

        public void LoadDataAsync(MapTema tema, Action loadAction) {
            _spriteDic?.Clear();
            _spriteDic = new Dictionary<TileType, Sprite>();
            IsLoad = false;
            string label = tema.ToString() + "Sprite";

            Debug.Log("Load 시작");


            DataManager.Instance.LoadAssetsByLabelAsync<Sprite>(label).ContinueWith((spriteList) => {
                foreach (Sprite sprite in spriteList) {
                    Match match = Regex.Match(sprite.name, @"\((\d+)\)");
                    if (match.Success) {
                        string numberStr = match.Groups[1].Value;
                        int number = int.Parse(numberStr);
                        IsLoad = true;
                        _spriteDic.Add((TileType)number, sprite);

                    } else {
                        Debug.Log("매칭 실패 :" + sprite.name);
                    }
                }
                loadAction?.Invoke();
                Debug.Log("로딩 완료");
            });
        }
        public static void ReleaseTema(MapTema tema) {
            string label = tema.ToString() + "Sprite";

            DataManager.Instance.ReleaseAssetsByLabel(label);
        }
      
    }
}

using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System;
using Zenject;

namespace Data
{
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
        private DataManager _dataManager;


        public TileSpriteMapper(DataManager dataManager) {
            _dataManager = dataManager;
        }

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


            _dataManager.LoadAssetsByLabelAsync<Sprite>(label).ContinueWith((spriteList) => {
                foreach (Sprite sprite in spriteList) {
                    Match match = Regex.Match(sprite.name, @"\((\d+)\)"); // 이름으로 매핑
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
        public void ReleaseTema(MapTema tema) {
            string label = tema.ToString() + "Sprite";

            _dataManager.ReleaseAssetsByLabel(label);
        }
    }
}

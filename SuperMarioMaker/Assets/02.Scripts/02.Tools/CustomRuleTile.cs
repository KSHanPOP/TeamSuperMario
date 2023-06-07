using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "CustomTiles/CustomRuleTile", fileName = "CustomRuleTile")]
public class CustomRuleTile : RuleTile<CustomRuleTile.Neighbor> {
    public bool customField;

    public string tileName;
    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Null = 3;
        public const int NotNull = 4;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.Null: return tile == null;
            case Neighbor.NotNull: return tile != null;
        }
        return base.RuleMatch(neighbor, tile);
    }

    public void SetTiles(Tilemap tilemap, Vector3Int position)
    {
        // Position Ÿ�� ����
        tilemap.SetTile(position, this);

        // ���� �� Ÿ�� ����
        Vector3Int leftPosition = new Vector3Int(position.x - 1, position.y, position.z);
        tilemap.SetTile(leftPosition, this);

        // ������ �� Ÿ�� ����
        Vector3Int rightPosition = new Vector3Int(position.x + 1, position.y, position.z);
        tilemap.SetTile(rightPosition, this);
    }
}
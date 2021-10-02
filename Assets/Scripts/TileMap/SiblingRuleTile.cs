using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/Custom/SiblingRuleTile")]
public class SiblingRuleTile : RuleTile<SiblingRuleTile.Neighbor> {

    public List<TileBase> siblings = new List<TileBase>();

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Sibling = 3;
        public const int NotSibling = 4;
    }

    public override bool RuleMatch(int neighbor, TileBase other) {
        switch (neighbor) {
            case Neighbor.Sibling: return other == this || siblings.Contains(other);
            case Neighbor.NotSibling: return other != this && !siblings.Contains(other);
        }
        return base.RuleMatch(neighbor, other);
    }
}

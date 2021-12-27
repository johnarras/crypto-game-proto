using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BaseBehaviour : MonoBehaviour
{
    private GameState _gs;

    protected GameState gs {  get { if (_gs == null) _gs = new GameState(); return _gs; } }

}
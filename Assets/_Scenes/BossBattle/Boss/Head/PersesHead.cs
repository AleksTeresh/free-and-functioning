using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PersesHead : SpawnHouse {

    private void OnDestroy()
    {
        if (player)
        {
            player.AddUnit(rangeSwarmling.name, transform.position, transform.position, transform.rotation, this);
        }
    }
}

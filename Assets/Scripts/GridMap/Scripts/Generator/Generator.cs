using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Assets.GridMap
{
    public class Generator : GridGenerator<MapCell>
    {
        private ActionBasedHeightMapGenerator actionHeightMapGenerator;
        private DelaunayGenerator delaunayGenerator;

        public Generator(ActionBasedGeneratorSettings generatorSettings)
        {
            this.actionHeightMapGenerator = new ActionBasedHeightMapGenerator(generatorSettings.heighMapGeneratorSettings);
        }

        public override MapCell GenerateCell(OnNewGridObjectGenerationArgs args)
        {
            float h = 0, hn = 0;
            if (actionHeightMapGenerator != null)
            {
                (h, hn) = actionHeightMapGenerator.CalculateHeightOnPoint(args);
            }
            return new MapCell { height = h, heightNormalised = hn };
        }

        public override void GenerateChunk(OnNewChunkGenerationArgs args)
        {
            actionHeightMapGenerator.OnNewChunkGeneration(args);
        }
    }
}
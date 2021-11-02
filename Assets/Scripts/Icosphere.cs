using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace growth {
    public class Icosphere {

        const float a = 0.8506507174597755f;
        const float b = 0.5257312591858783f;

        public static readonly Vector3[] vertices = {
            new Vector3(-a, -b,  0), new Vector3(-a,  b,  0), new Vector3( -b,  0, -a), new Vector3( -b,  0,  a),
            new Vector3( 0, -a, -b), new Vector3( 0, -a,  b), new Vector3( 0,  a, -b), new Vector3( 0,  a,  b),
            new Vector3( b,  0, -a), new Vector3( b,  0,  a), new Vector3( a, -b,  0), new Vector3( a,  b,  0)
        };

        public static readonly int[,] triangles ={
            { 0,  3,  1}, { 1,  3,  7}, { 2,  0,  1}, { 2,  1,  6},
            { 4,  0,  2}, { 4,  5,  0}, { 5,  3,  0}, { 6,  1,  7},
            { 6,  7, 11}, { 7,  3,  9}, { 8,  2,  6}, { 8,  4,  2},
            { 8,  6, 11}, { 8, 10,  4}, { 8, 11, 10}, { 9,  3,  5},
            { 10,  5,  4}, { 10,  9,  5}, { 11,  7,  9}, { 11,  9, 10},
        };

        public static readonly int[,] edges = {
            { 3,1,2,4,5 }, {3,0,2,6,7}, {0,1,6,4,8}, {0,1,7,5,9},
            {0,2,5,8,10 }, {4,0,3,9,10},{1,2,7,11,8},{1,3,6,11,9},
            {2,6,4,11,10}, {7,3,5,10,11}, {8,4,11,5,9}, {6,7,8,10,9}
        };
    }
}

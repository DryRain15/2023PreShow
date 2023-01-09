using System.Collections.Generic;
using UnityEngine;
using Proto.BasicExtensionUtils;

namespace Proto.CustomDebugTool
{
    public enum GizmoType
    {
        Box,
        FilledBox,
        Circle,
        FilledCircle,
        Line
    }

    public class CustomDebugger : MonoBehaviour
    {
        public delegate void OnDrawBoxGizmosDelegate(Vector3 center, Vector3 size);

        public OnDrawBoxGizmosDelegate DrawBoxGizmosDelegate;

        public static CustomDebugger Instance;

        public List<Dictionary<string, object>> DrawGizmoData;

        private void Awake()
        {
            Instance = this;
            DrawGizmoData ??= new List<Dictionary<string, object>>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        #region Adders


        public void AddDrawBoxGizmo(Vector3 center, Vector3 size, Color? c = null)
        {
            var data = new Dictionary<string, object>();
            data.Add("type", GizmoType.Box);
            data.Add("center", center);
            data.Add("size", size);
            if (c != null) data.Add("color", c);
            DrawGizmoData.Add(data);
        }

        public void AddDrawBoxGizmo(Bounds bounds, Color? c = null)
        {
            AddDrawBoxGizmo(bounds.center, bounds.size, c);
        }

        public void AddDrawFilledBoxGizmo(Vector3 center, Vector3 size, Color? c = null)
        {
            var data = new Dictionary<string, object>();
            data.Add("type", GizmoType.FilledBox);
            data.Add("center", center);
            data.Add("size", size);
            if (c != null) data.Add("color", c);
            DrawGizmoData.Add(data);
        }

        public void AddDrawFilledBoxGizmo(Bounds bounds, Color? c = null)
        {
            AddDrawFilledBoxGizmo(bounds.center, bounds.size, c);
        }

        public void AddDrawCircleGizmo(Vector3 center, float radius, Color? c = null)
        {
            var data = new Dictionary<string, object>();
            data.Add("type", GizmoType.Circle);
            data.Add("center", center);
            data.Add("radius", radius);
            if (c != null) data.Add("color", c);
            DrawGizmoData.Add(data);
        }


        public void AddDrawFilledCircleGizmo(Vector3 center, float radius, Color? c = null)
        {
            var data = new Dictionary<string, object>();
            data.Add("type", GizmoType.FilledCircle);
            data.Add("center", center);
            data.Add("radius", radius);
            if (c != null) data.Add("color", c);
            DrawGizmoData.Add(data);
        }

        public void AddDrawLineGizmo(Vector3 from, Vector3 to, Color? c = null)
        {
            var data = new Dictionary<string, object>();
            data.Add("type", GizmoType.Circle);
            data.Add("from", from);
            data.Add("to", to);
            if (c != null) data.Add("color", c);
            DrawGizmoData.Add(data);
        }

        #endregion

        #region Drawers

        private void DrawLineGizmo(Vector3 from, Vector3 to)
        {
            Gizmos.DrawLine(from, to);
        }

        private void DrawCircleGizmo(Vector3 center, float radius)
        {
            Gizmos.DrawWireSphere(center, radius);
        }

        private void DrawFilledCircleGizmo(Vector3 center, float radius)
        {
            Gizmos.DrawSphere(center, radius);
        }

        private void DrawBoxGizmo(Vector3 center, Vector3 size)
        {
            Gizmos.DrawWireCube(center, size);
        }

        private void DrawBoxGizmo(Bounds bounds)
        {
            DrawBoxGizmo(bounds.center, bounds.size);
        }

        private void DrawFilledBoxGizmo(Vector3 center, Vector3 size)
        {
            Gizmos.DrawCube(center, size);
        }

        private void DrawFilledBoxGizmo(Bounds bounds)
        {
            DrawFilledBoxGizmo(bounds.center, bounds.size);
        }

        #endregion

        private void OnDrawGizmos()
        {
            // 요청이 들어온 Gizmo Call Data들을 순회한 뒤 Flush
            while (DrawGizmoData?.Count > 0)
            {
                var data = DrawGizmoData[0];
                if (data["type"] is GizmoType type)
                {
                    Gizmos.color = (Color) data.GetDefault("color", Color.red);
                    switch (type)
                    {
                        case GizmoType.Box:
                            if (data.GetDefault("center", Vector3.zero) is Vector3 box_center
                                && data.GetDefault("size", Vector3.zero) is Vector3 box_size)
                                DrawBoxGizmo(box_center, box_size);
                            else if (data.GetDefault("bound",
                                new Bounds(Vector3.zero, Vector3.zero)) is Bounds bounds)
                                DrawBoxGizmo(bounds);
                            break;
                        case GizmoType.FilledBox:
                            if (data.GetDefault("center", Vector3.zero) is Vector3 fbox_center
                                && data.GetDefault("size", Vector3.zero) is Vector3 fbox_size)
                                DrawFilledBoxGizmo(fbox_center, fbox_size);
                            else if (data.GetDefault("bound",
                                new Bounds(Vector3.zero, Vector3.zero)) is Bounds bounds)
                                DrawFilledBoxGizmo(bounds);
                            break;
                        case GizmoType.Circle:
                            if (data.GetDefault("center", Vector3.zero) is Vector3 circle_center
                                && data.GetDefault("radius", 0f) is float radius)
                                DrawCircleGizmo(circle_center, radius);
                            break;
                        case GizmoType.FilledCircle:
                            if (data.GetDefault("center", Vector3.zero) is Vector3 fcircle_center
                                && data.GetDefault("radius", 0f) is float fradius)
                                DrawFilledCircleGizmo(fcircle_center, fradius);
                            break;
                        case GizmoType.Line:
                            if (data.GetDefault("from", Vector3.zero) is Vector3 from
                                && data.GetDefault("to", Vector3.zero) is Vector3 to)
                                DrawLineGizmo(from, to);
                            break;
                        default:
                            Debug.LogWarning("This is Not Registered Gizmo Type");
                            break;
                    }
                }

                DrawGizmoData.RemoveAt(0);
            }
        }
    }

}
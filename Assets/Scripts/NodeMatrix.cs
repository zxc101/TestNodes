using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMatrix : MonoBehaviour
{
    private void Awake()
    {
        //Debug.Log(NodeSetting.phone.position);
        //transform.position = NodeSetting.phone.position;
    }

    public void Start()
    {
        transform.position = NodeSetting.phone.position + Vector3.up * 3;
        CreateNodes();
    }

    private void Update()
    {
        MoveToPhone();
    }

    public void CreateNodes()
    {
        // Если количество Raycast-ов чётное количество
        if (NodeSetting.count % 2 == 0)
        {
            CreateEvenCountNodes();
        }
        // Иначе, если количество Raycast-ов не чётное количество
        else
        {
            CreateOddCountNodes();
        }
    }

    /// <summary>
    /// Создать чётное количество нодов
    /// </summary>
    private void CreateEvenCountNodes()
    {
        // Проходим по всем x
        for (int i = -(NodeSetting.count / 2); i < NodeSetting.count / 2; i++)
        {
            // Проходим по всем z
            for (int j = -(NodeSetting.count / 2); j < NodeSetting.count / 2; j++)
            {
                // Берём позиции сталкивающиеся с маской (заменить на Frame) и добавляем их в базу
                AddNodes(RaycastAll(new Vector3(transform.position.x + NodeSetting.wolkDistance * i + NodeSetting.wolkDistance * 0.5f,
                                                transform.position.y,
                                                transform.position.z + NodeSetting.wolkDistance * j + NodeSetting.wolkDistance * 0.5f)),
                                    i, j);
            }
        }
    }

    /// <summary>
    /// Создать не чётное количество нодов
    /// </summary>
    private void CreateOddCountNodes()
    {
        // Проходим по всем x
        for (int i = -(NodeSetting.count / 2); i <= NodeSetting.count / 2; i++)
        {
            // Проходим по всем z
            for (int j = -(NodeSetting.count / 2); j <= NodeSetting.count / 2; j++)
            {
                // Берём позиции сталкивающиеся с маской (заменить на Frame) и добавляем их в базу
                AddNodes(RaycastAll(new Vector3(transform.position.x + NodeSetting.wolkDistance * i,
                                                transform.position.y,
                                                transform.position.z + NodeSetting.wolkDistance * j)),
                                    i, j);
            }
        }
    }

    /// <summary>
    /// Берём позиции сталкивающиеся с маской (заменить на Frame)
    /// </summary>
    /// <param name="position">Позиция начала луча</param>
    /// <returns></returns>
    private RaycastHit[] RaycastAll(Vector3 position)
    {
        // Берём позиции сталкивающиеся с маской (заменить на Frame)
        return Physics.RaycastAll(position, Vector3.down, Mathf.Infinity, NodeSetting.layerMask);
    }

    /// <summary>
    /// Добавляем ноды
    /// </summary>
    /// <param name="hits">Столкнувшиеся Node</param>
    /// <param name="X">нумерация по x</param>
    /// <param name="Z">нумерация по z</param>
    private void AddNodes(RaycastHit[] hits)
    {
        // Проходим по всему столкнувшемуся
        for (int i = 0; i < hits.Length; i++)
        {
            // Добавляем позицию в базу
            NodeList.Update(hits[i].point);
        }
    }

    /// <summary>
    /// Добавляем ноды
    /// </summary>
    /// <param name="hits">Столкнувшиеся Node</param>
    /// <param name="X">нумерация по x</param>
    /// <param name="Z">нумерация по z</param>
    private void AddNodes(RaycastHit[] hits, float X, float Z)
    {
        // Проходим по всему столкнувшемуся
        for (int i = 0; i < hits.Length; i++)
        {
            // Добавляем позицию в базу
            NodeList.Update(new Vector3(transform.position.x + NodeSetting.wolkDistance * X,
                                     hits[i].transform.position.y,
                                     transform.position.z + NodeSetting.wolkDistance * Z));
        }
    }

    private void MoveToPhone()
    {
        Vector3 phoneDistance = NodeSetting.phone.position - transform.position;

        if (Mathf.Abs(NodeSetting.phone.position.x - transform.position.x) >= NodeSetting.wolkDistance)
        {
            transform.position += Vector3.right * NodeSetting.wolkDistance * (phoneDistance.x / Mathf.Abs(phoneDistance.x));
            CreateNodes();
        }

        if (Mathf.Abs(NodeSetting.phone.position.z - transform.position.z) >= NodeSetting.wolkDistance)
        {
            transform.position += Vector3.forward * NodeSetting.wolkDistance * (phoneDistance.z / Mathf.Abs(phoneDistance.z));
            CreateNodes();
        }

        transform.position = new Vector3(transform.position.x,
                                         NodeSetting.phone.position.y + 3,
                                         transform.position.z);
    }
}

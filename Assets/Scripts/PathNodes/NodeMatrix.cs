using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMatrix : MonoBehaviour
{
    private void Start()
    {
        transform.position = NodeSetting.phone.position + Vector3.up * 3;
        CreateNodes();
        StartCoroutine(CMoveToPhone());
    }

    private IEnumerator CMoveToPhone()
    {
        while (true)
        {
            MoveToPhone();
            yield return new WaitForFixedUpdate();
        }
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
                Helper.helpVector.x = transform.position.x + NodeSetting.wolkDistance * i + NodeSetting.wolkDistance * 0.5f;
                Helper.helpVector.y = transform.position.y;
                Helper.helpVector.z = transform.position.z + NodeSetting.wolkDistance * j + NodeSetting.wolkDistance * 0.5f;
                // Берём позиции сталкивающиеся с маской (заменить на Frame) и добавляем их в базу
                AddNodes(RaycastAll(Helper.helpVector), i, j);
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
                Helper.helpVector.x = transform.position.x + NodeSetting.wolkDistance * i;
                Helper.helpVector.y = transform.position.y;
                Helper.helpVector.z = transform.position.z + NodeSetting.wolkDistance * j;

                AddNodes(RaycastAll(Helper.helpVector), i, j);
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
            Helper.helpVector.x = transform.position.x + NodeSetting.wolkDistance * X;
            Helper.helpVector.y = hits[i].transform.position.y;
            Helper.helpVector.z = transform.position.z + NodeSetting.wolkDistance * Z;

            NodeList.Update(Helper.helpVector);
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

        Helper.helpVector.x = transform.position.x;
        Helper.helpVector.y = NodeSetting.phone.position.y + 3;
        Helper.helpVector.z = transform.position.z;

        transform.position = Helper.helpVector;
    }
}
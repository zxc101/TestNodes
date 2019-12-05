using GoogleARCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Helpers;
using Pets;

public class PlaneCreater : MonoBehaviour
{
    [SerializeField] private Camera ARCamera;

    [SerializeField] private Button createMap_BTN;
    [SerializeField] private Button play_BTN;

    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private Transform point3;

    [SerializeField] private Transform plane;

    [SerializeField] private Pet pet;

    [SerializeField] private Transform goal;
    [SerializeField] private Transform goalsBase;
    [SerializeField] private Text textNextNeed;

    private Touch touch;
    private TrackableHit hit;
    private TrackableHitFlags filter;
    private bool isPlay;

    private Transform Need
    {
        get
        {
            for(int i = 0; i < pet.Needs.Length; i++)
            {
                if (!pet.Needs[i].prefab.gameObject.activeSelf)
                {
                    return pet.Needs[i].prefab;
                }
            }
            return null;
        }
    }

    private void Start()
    {
        //CreateMap_Play_Trigger();
        filter = TrackableHitFlags.PlaneWithinPolygon |
            TrackableHitFlags.FeaturePointWithSurfaceNormal;
    }

    private void Update()
    {
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            return;
        }

        if (Frame.Raycast(touch.position.x, touch.position.y, filter, out hit))
        {
            if (!((hit.Trackable is DetectedPlane) &&
                Vector3.Dot(ARCamera.transform.position - hit.Pose.position,
                    hit.Pose.rotation * Vector3.up) < 0))
            {
                if (hit.Trackable is DetectedPlane)
                {
                    if (isPlay)
                    {
                        if (pet.gameObject.activeSelf)
                        {
                            if(Need == null)
                            {
                                pet.Goals.AddFirst(Instantiate(goal, goalsBase));
                                pet.Goals.First.position = hit.Pose.position;
                                pet.Goals.First.Rotate(0, 180, 0, Space.Self);
                                WriteNextNeed();
                            }
                            else
                            {
                                Need.position = hit.Pose.position;
                                Need.Rotate(0, 180, 0, Space.Self);
                                Need.gameObject.SetActive(true);
                                WriteNextNeed();
                            }
                            //if (items.Count > 0)
                            //{
                            //    textNextItem.text = $"{items.Count}";
                            //    items.Peek().transform.position = hit.Pose.position;
                            //    items.Peek().transform.Rotate(0, 180, 0, Space.Self);
                            //    items.Peek().transform.gameObject.SetActive(true);
                            //}
                            //else
                            //{
                            //    pet.Goals.AddFirst(Instantiate(goal, goalsBase));

                            //    pet.Goals.First.position = hit.Pose.position;
                            //    pet.Goals.First.Rotate(0, 180, 0, Space.Self);
                            //    pet.Goals.First.gameObject.SetActive(true);

                            //    textNextItem.text = items.Peek().name;
                            //}
                        }
                        else
                        {
                            pet.transform.position = hit.Pose.position;
                            pet.transform.Rotate(0, 180, 0, Space.Self);
                            pet.transform.gameObject.SetActive(true);
                            WriteNextNeed();
                        }
                        //Transform newItem;
                        //if (items.Count == 0)
                        //{
                        //    pet.Goals.AddFirst(Instantiate(goal, goalsBase));
                        //    newItem = pet.Goals.First;
                        //    textNextItem.text = "Auxiliary goal";
                        //}
                        //else
                        //{
                        //    newItem = items.Dequeue();
                        //    textNextItem.text = items.Peek().name;
                        //}
                        //newItem.rotation = hit.Pose.rotation;
                        //newItem.Rotate(0, 180, 0, Space.Self);
                        //newItem.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (!point1.gameObject.activeSelf && !point2.gameObject.activeSelf && !point3.gameObject.activeSelf)
                        {
                            // Ставим первую точку
                            SetPoint(ref point1, hit);
                        }
                        else if (point1.gameObject.activeSelf && !point2.gameObject.activeSelf && !point3.gameObject.activeSelf)
                        {
                            // Ставим вторую точку
                            SetPoint(ref point2, hit);
                        }
                        else if (point1.gameObject.activeSelf && point2.gameObject.activeSelf && !point3.gameObject.activeSelf)
                        {
                            SetPoint(ref point3, hit);

                            Transform newPlane = Instantiate(plane).transform;

                            var rotation1 = point1.rotation;
                            point1.LookAt(point3);
                            newPlane.rotation = point1.rotation;
                            point1.rotation = rotation1;

                            Helper.helpVector.x = (point1.position.x + point2.position.x) / 2;
                            Helper.helpVector.y = (point1.position.y + point2.position.y) / 2;
                            Helper.helpVector.z = (point1.position.z + point2.position.z) / 2;
                            newPlane.position = Helper.helpVector;

                            Helper.helpVector.x = Vector3.Distance(point2.position, point3.position);
                            Helper.helpVector.y = plane.localScale.y;
                            Helper.helpVector.z = Vector3.Distance(point1.position, point3.position);
                            newPlane.localScale = Helper.helpVector;


                            DetectedPlane detectedPlane = hit.Trackable as DetectedPlane;

                            if (detectedPlane.PlaneType == DetectedPlaneType.Vertical)
                            {
                                newPlane.Rotate(newPlane.rotation.x, newPlane.rotation.y, newPlane.rotation.z + 90);
                            }

                            HidePoints();
                        }
                    }
                }
            }
        }
    }

    private void WriteNextNeed()
    {
        if (Need != null)
        {
            textNextNeed.text = Need.name;
        }
        else
        {
            textNextNeed.text = "Auxiliary goal";
        }
    }

    public void CreateMap()
    {
        isPlay = false;
        CreateMap_Play_Trigger();
    }

    public void Play()
    {
        isPlay = true;
        //RenqueueAllItems();
        CreateMap_Play_Trigger();
        textNextNeed.text = pet.name;
    }

    private void CreateMap_Play_Trigger()
    {
        play_BTN.gameObject.SetActive(!isPlay);
        createMap_BTN.gameObject.SetActive(isPlay);

        if (!isPlay)
        {
            textNextNeed.text = "Map creater";

            //        for (int i = 0; i < pet.Needs.Length; i++)
            //        {
            //            pet.Needs[i].prefab.gameObject.SetActive(false);
            //        }

            //        pet.gameObject.SetActive(false);

            //        items.Clear();

            //        NodeList.nodeList.Clear();
            //        NodeList.rawNodeList.Clear();

            //        HideAllItems();
        }
        else
        {
            textNextNeed.text = "Game";

            //        RenqueueAllItems();
            //        if (items.Count > 0)
            //        {
            //            textNextItem.text = items.Peek().name;
            //        }
        }
    }

    //private void RenqueueAllItems()
    //{
    //    HideAllItems();
    //    items.Enqueue(pet.Transform);
    //    for (int i = 0; i < pet.Needs.Length; i++)
    //    {
    //        items.Enqueue(pet.Needs[i].prefab);
    //    }
    //}

    //private void HideAllItems()
    //{
    //    pet.gameObject.SetActive(false);
    //    for (int i = 0; i < pet.Needs.Length; i++)
    //    {
    //        pet.Needs[i].prefab.gameObject.SetActive(false);
    //    }
    //    items.Clear();
    //}

    private void SetPoint(ref Transform point, TrackableHit hit)
    {
        point.position = hit.Pose.position;
        point.rotation = hit.Pose.rotation;
        point.gameObject.SetActive(true);
    }

    private void HidePoints()
    {
        point1.gameObject.SetActive(false);
        point2.gameObject.SetActive(false);
        point3.gameObject.SetActive(false);
    }

    private void CreatePlane()
    {
        Transform newPlane = Instantiate(plane).transform;

        // Position
        Helper.helpVector.x = (point1.position.x + point2.position.x) / 2;
        Helper.helpVector.y = (point1.position.y + point2.position.y) / 2;
        Helper.helpVector.z = (point1.position.z + point2.position.z) / 2;
        newPlane.position = Helper.helpVector;

        // Rotation
        //newPlane.rotation = point1.rotation;

        // Scale
        Helper.helpVector.x = point2.position.x - point1.position.x;
        Helper.helpVector.y = plane.localScale.y;
        Helper.helpVector.z = point2.position.z - point1.position.z;
        newPlane.localScale = Helper.helpVector;
    }
}

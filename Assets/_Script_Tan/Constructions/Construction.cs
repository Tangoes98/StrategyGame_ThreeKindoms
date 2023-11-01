using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Construction : MonoBehaviour
{
    // Is this construction occupied by enemy unit
    [SerializeField] protected bool _isCapByEnemy;
    [SerializeField] protected bool _isCapByAlly;

    [SerializeField] protected bool _isFlammable;
    [SerializeField] protected bool _canBeAccessedByFriendlyUnit;
    [SerializeField] protected bool _canBeAccessedByEnemyUnit;

    GridPosition _constructionGridPosition;
    ConstructionHealthSystem _constructionHealth;


    public enum ConstructionOccupationConditionType
    {
        Neutral, Enemy, Friend
    }

    [Serializable]
    public struct ConstructionConditions
    {
        public ConstructionOccupationConditionType _condition;
        public Material _material;
    }

    [SerializeField] ConstructionOccupationConditionType _currentCondition;
    [SerializeField] List<ConstructionConditions> _conditionTypeList;







    protected virtual void Start()
    {
        _constructionGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        _constructionHealth = GetComponent<ConstructionHealthSystem>();

        _constructionHealth.OnDestroyed += ConstructionHealth_OnDestroyed;
    }

    protected virtual void Update()
    {
        UpdateConstructionCondition(_currentCondition);
        ConstructionOccupationConditionValidation();
    }








    void ConstructionHealth_OnDestroyed()
    {
        LevelGrid.Instance.RemoveConstructionFromGrdObject(_constructionGridPosition, this);
        Destroy(gameObject);
    }












    void ConstructionOccupationConditionValidation()
    {
        GridPosition currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.GetGridObject(currentGridPosition).UpdateConstructionType();


    }




    void UpdateConstructionCondition(ConstructionOccupationConditionType conditionType)
    {
        foreach (ConstructionConditions condition in _conditionTypeList)
        {
            if (condition._condition == conditionType) transform.GetComponent<MeshRenderer>().material = condition._material;
        }
    }

    public void SetConstructionOccupationCondition(ConstructionOccupationConditionType condition) => _currentCondition = condition;






}

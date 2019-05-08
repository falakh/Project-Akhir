using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Selector : Node {
    /** The child nodes for this selector */
    protected List<Node> m_nodes = new List<Node>();


    /** The constructor requires a lsit of child nodes to be 
     * passed in*/
    public Selector(List<Node> nodes, string name) {
        m_nodes = nodes;
        this.name = name;
    }

    /* If any of the children reports a success, the selector will
     * immediately report a success upwards. If all children fail,
     * it will report a failure instead.*/
    public override NodeStates Evaluate() {
        foreach (Node node in m_nodes) {

            

            switch (node.Evaluate()) {
                case NodeStates.FAILURE:
                    Debug.Log(node.name + " : fail");
                    continue;
                case NodeStates.SUCCESS:
                    m_nodeState = NodeStates.SUCCESS;
                    Debug.Log(node.name + " : success");
                    return m_nodeState;
                case NodeStates.RUNNING:
                    m_nodeState = NodeStates.RUNNING;
                    Debug.Log(node.name + " : running");
                    return m_nodeState;
                default:
                    continue;
            }
        }
        m_nodeState = NodeStates.FAILURE;
        Debug.Log(this.name+ " : failure");
        return m_nodeState;
    }
}

using UnityEngine;
using System;

public class CharacterLinkedList
{
    public class Node
    {
        public CharacterData item;
        public Node next;
        public Node prev;

        public Node(CharacterData data)
        {
            item = data;
            next = null;
            prev = null;
        }
    }

    public class DoublyLinkedList
    {
        private Node startNode;

        public DoublyLinkedList()
        {
            startNode = null;
        }

        public void InsertInEmptyList(CharacterData data)
        {
            if (startNode == null)
            {
                Node newNode = new Node(data);
                startNode = newNode;
            }
            else
            {
                Debug.Log("List is not empty");
            }
        }

        public void InsertAtStart(CharacterData data)
        {
            if (startNode == null)
            {
                Node newNode = new Node(data);
                startNode = newNode;
                Debug.Log("Node inserted");
                return;
            }

            Node newNode2 = new Node(data);
            newNode2.next = startNode;
            startNode.prev = newNode2;
            startNode = newNode2;
        }

        public void InsertAtEnd(CharacterData data)
        {
            if (startNode == null)
            {
                Node newNode = new Node(data);
                startNode = newNode;
                return;
            }

            Node n = startNode;
            while (n.next != null)
            {
                n = n.next;
            }

            Node newNode2 = new Node(data);
            n.next = newNode2;
            newNode2.prev = n;
        }

        public void InsertAfterItem(CharacterData x, CharacterData data)
        {
            if (startNode == null)
            {
                Debug.Log("List is empty");
                return;
            }

            Node n = startNode;
            while (n != null)
            {
                if (n.item == x)
                    break;
                n = n.next;
            }

            if (n == null)
            {
                Debug.Log("Item not in the list");
            }
            else
            {
                Node newNode = new Node(data);
                newNode.prev = n;
                newNode.next = n.next;

                if (n.next != null)
                    n.next.prev = newNode;

                n.next = newNode;
            }
        }

        public void InsertBeforeItem(CharacterData x, CharacterData data)
        {
            if (startNode == null)
            {
                Debug.Log("List is empty");
                return;
            }

            Node n = startNode;
            while (n != null)
            {
                if (n.item == x)
                    break;
                n = n.next;
            }

            if (n == null)
            {
                Debug.Log("Item not in the list");
            }
            else
            {
                Node newNode = new Node(data);
                newNode.next = n;
                newNode.prev = n.prev;

                if (n.prev != null)
                    n.prev.next = newNode;

                n.prev = newNode;

                if (n == startNode)
                    startNode = newNode;
            }
        }

        public void TraverseList()
        {
            if (startNode == null)
            {
                Debug.Log("List has no element");
                return;
            }

            Node n = startNode;
            while (n != null)
            {
                Debug.Log(n.item.characterName + " ");
                n = n.next;
            }
        }

        public void DeleteAtStart()
        {
            if (startNode == null)
            {
                Debug.Log("The list has no element to delete");
                return;
            }

            if (startNode.next == null)
            {
                startNode = null;
                return;
            }

            startNode = startNode.next;
            startNode.prev = null;
        }

        public void DeleteAtEnd()
        {
            if (startNode == null)
            {
                Debug.Log("The list has no element to delete");
                return;
            }

            if (startNode.next == null)
            {
                startNode = null;
                return;
            }

            Node n = startNode;
            while (n.next != null)
            {
                n = n.next;
            }

            n.prev.next = null;
        }

        public void DeleteElementByValue(CharacterData x)
        {
            if (startNode == null)
            {
                Debug.Log("The list has no element to delete");
                return;
            }

            if (startNode.next == null)
            {
                if (startNode.item == x)
                    startNode = null;
                else
                    Debug.Log("Item not found");
                return;
            }

            if (startNode.item == x)
            {
                startNode = startNode.next;
                startNode.prev = null;
                return;
            }

            Node n = startNode;
            while (n.next != null)
            {
                if (n.item == x)
                    break;
                n = n.next;
            }

            if (n.next != null)
            {
                n.prev.next = n.next;
                n.next.prev = n.prev;
            }
            else
            {
                if (n.item == x)
                    n.prev.next = null;
                else
                    Debug.Log("Element not found");
            }
        }

        public void ReverseLinkedList()
        {
            if (startNode == null)
            {
                Debug.Log("The list has no element to delete");
                return;
            }

            Node p = startNode;
            Node q = p.next;
            p.next = null;
            p.prev = q;

            while (q != null)
            {
                q.prev = q.next;
                q.next = p;
                p = q;
                q = q.prev;
            }

            startNode = p;
        }

        public Node GetCurrentNode()
        {
            return startNode;
        }

        public Node GetNextNode(Node current)
        {
            if (current != null && current.next != null)
                return current.next;
            return startNode;
        }

        public Node GetPreviousNode(Node current)
        {
            if (current == null || current == startNode)
            {
                Node last = startNode;
                while (last != null && last.next != null)
                {
                    last = last.next;
                }
                return last;
            }
            return current.prev;
        }

        public int GetCount()
        {
            if (startNode == null) return 0;

            int count = 0;
            Node n = startNode;
            while (n != null)
            {
                count++;
                n = n.next;
            }
            return count;
        }

        public Node FindByID(int characterID)
        {
            if (startNode == null) return null;

            Node n = startNode;
            while (n != null)
            {
                if (n.item.characterID == characterID)
                    return n;
                n = n.next;
            }
            return null;
        }
    }
}

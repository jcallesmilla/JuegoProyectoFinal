using UnityEngine;
using System;
public class ListaDoblementeEnlazada
{
public class Node
{
    public int item;
    public Node next;
    public Node prev;

    public Node(int data)
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

    // Insert into empty list
    public void InsertInEmptyList(int data)
    {
        if (startNode == null)
        {
            Node newNode = new Node(data);
            startNode = newNode;
        }
        else
        {
            Console.WriteLine("List is not empty");
        }
    }

    // Insert at start
    public void InsertAtStart(int data)
    {
        if (startNode == null)
        {
            Node newNode = new Node(data);
            startNode = newNode;
            Console.WriteLine("Node inserted");
            return;
        }

        Node newNode2 = new Node(data);
        newNode2.next = startNode;
        startNode.prev = newNode2;
        startNode = newNode2;
    }

    // Insert at end
    public void InsertAtEnd(int data)
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

    // Insert after item
    public void InsertAfterItem(int x, int data)
    {
        if (startNode == null)
        {
            Console.WriteLine("List is empty");
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
            Console.WriteLine("Item not in the list");
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

    // Insert before item
    public void InsertBeforeItem(int x, int data)
    {
        if (startNode == null)
        {
            Console.WriteLine("List is empty");
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
            Console.WriteLine("Item not in the list");
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

    // Traverse list
    public void TraverseList()
    {
        if (startNode == null)
        {
            Console.WriteLine("List has no element");
            return;
        }

        Node n = startNode;
        while (n != null)
        {
            Console.WriteLine(n.item + " ");
            n = n.next;
        }
    }

    // Delete at start
    public void DeleteAtStart()
    {
        if (startNode == null)
        {
            Console.WriteLine("The list has no element to delete");
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

    // Delete at end
    public void DeleteAtEnd()
    {
        if (startNode == null)
        {
            Console.WriteLine("The list has no element to delete");
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

    // Delete element by value
    public void DeleteElementByValue(int x)
    {
        if (startNode == null)
        {
            Console.WriteLine("The list has no element to delete");
            return;
        }

        if (startNode.next == null)
        {
            if (startNode.item == x)
                startNode = null;
            else
                Console.WriteLine("Item not found");
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
                Console.WriteLine("Element not found");
        }
    }

    // Reverse list
    public void ReverseLinkedList()
    {
        if (startNode == null)
        {
            Console.WriteLine("The list has no element to delete");
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
}    
}

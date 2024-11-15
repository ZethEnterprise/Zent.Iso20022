<Query Kind="Program" />

void Main()
{
	A a = new D();
	
	var bb = a as A.B;
	bb.Dump();
	
	E e = new E
	{
		AE = new A.B()
	};
	
	switch(e.AE)
	{
		case A.B b:
		"test".Dump();
		break;
		default:
		"no".Dump();
		break;
	}
	
	e.Dump();
}

// You can define other methods, fields, classes and namespaces here
public abstract class A
{
	
	public class B : A
	{
	}
	
	public class C : A
	{
	}
}

public class D : A
{

}

public class E
{
	public A AE {get; set; }
}
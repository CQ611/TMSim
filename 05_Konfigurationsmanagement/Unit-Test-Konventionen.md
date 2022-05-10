# Unit-Test Code-Konventionen

## Aufbau der Testprojekte und Testklassen
- Zu jedem Hauptprojekt (= zu testendes Projekt) sollte ein dazugehöriges MSTest-Projekt mit dem Namenszusatz *.Tests* angelegt werden.  
(z.B. Turingmaschine.Core => Turingmaschine.Core<b>.Tests</b>)
- Im Hauptprojekt sollte jede Klasse separat in einer eigenen Datei definiert sein. (z.B. Klasse **Alphabet** in **Alphabet.cs**)
- Alle Tests zu einer Klasse sind in einer Testklasse zu finden, die die gleichen Namen trägt, ergänzt durch "Test".  
(z.B. alle Tests der Klasse **Alphabet** sind in der Testklasse **AlphabetTests** zu finden, welche als **AlphabetTests.cs** abgespeichert ist.)
- Das Testprojekt sollte die Ordnerstruktur aufweisen, wie das zu testende Projekt.
- Jede Testklasse muss **public** sein und das Attribut **[TestClass]** tragen.

## Aufbau der Testmethoden
- Jede Testmethode muss **public** sein und das Attribut **[TestMethod]** tragen.
- Alle Testmethoden sollten nach folgender Namenskonvention benannt werden:  
**\<Methodenname>\_\<Eingabeparameter / Bedingungen>\_\<Erwartetes Ergebnis>**  
(z.B. WordIsContainedIn_AlphaABCInputABBCCC_ReturnsTrue)
- Jede Testmethode sollte sich in drei Teile untergliedern lassen können:  
**Arrange**: Vorbereitung des Tests (Instanziierung und sofern sinnvoll konstante Zeichenfolgen)  
**Act**: Aufruf der zu testenden Methode; Wertzuweisung in Variable "actual"  
**Assert**: Ausführung des Tests. Vergleich des Sollwerts mit dem in **Act** ermittelten Wert
- Innerhalb der Testklasse können weitere (private) Hilfsmethoden definiert werden. (z.B. falls die Instanziierung aufwendiger ist und für mehrere Tests benötigt wird, kann diese ausgelagert werden.)

## Beispiel
```CSharp
[TestClass]
public class AlphabetTests
{
    [TestMethod]
    public void WordIsContainedIn_AlphaABCInputABBCCC_ReturnsTrue()
    {
        //Arrange
        Alphabet a = new Alphabet("ABC");
        
        //Act
        bool isContained = a.WordIsContainedIn("ABBCCC");
        
        //Assert
        Assert.IsTrue(isContained);
    }
}
```

## Wichtige Assert-Methoden (TODO...)
- **Assert.IsTrue(___)** für Boolsche-Werte
- **Assert.Equals(object objA, object objB);** z.B. für Strings

## Betrachtete Frameworks
- **MSTest** (ausgewählt, aufgrund guter Dokumentation und Standard)
- NUnit
- xUnit

## Quellen
- https://docs.microsoft.com/de-de/dotnet/core/testing/unit-testing-best-practices
- https://www.testim.io/blog/unit-testing-best-practices/

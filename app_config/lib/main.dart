import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:calendar_date_picker2/calendar_date_picker2.dart'; // Importar la librería

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Configuración Evento Rally',
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: Colors.greenAccent),
        useMaterial3: true,
      ),
      home: const MyHomePage(title: 'Configuración Evento Rally'),
    );
  }
}

class MyHomePage extends StatefulWidget {
  const MyHomePage({super.key, required this.title});

  final String title;

  @override
  State<MyHomePage> createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  // Fechas seleccionadas
  DateTime? _firstDate;
  DateTime? _secondDate;

  // Define el estado de los checkboxes usando un Map
  final Map<String, bool> _labCheckStates = {
    '1': false,
    '2': false,
    '3': false,
    '4': false,
    '5': false,
  };

  Widget laboratoryBoxCheck({
    required String name,
    required String id,
  }) {
    return SizedBox(
      width: MediaQuery.of(context).size.width * 0.40,
      child: Card(
        child: ListTile(
          leading: Icon(CupertinoIcons.trash),
          subtitle: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Padding(
                padding: const EdgeInsets.only(left: 10),
                child: Text(name),
              ),
              const Spacer(),
              Checkbox(
                value: _labCheckStates[id],
                onChanged: (bool? value) {
                  setState(() {
                    _labCheckStates[id] = value ?? false;
                  });
                },
              ),
            ],
          ),
        ),
      ),
    );
  }

  // Función para seleccionar una fecha
  Future<void> _selectFirstDate(BuildContext context) async {
    final List<DateTime?>? pickedDate = await showCalendarDatePicker2Dialog(
      context: context,
      config: CalendarDatePicker2WithActionButtonsConfig(
        firstDate: DateTime.now().add(
            const Duration(days: 1)), // La fecha debe ser mayor a la actual
        lastDate: DateTime(2101),
        calendarType: CalendarDatePicker2Type.single, // Solo permite una fecha
      ),
      dialogSize: const Size(325, 400),
      value: [_firstDate], // Pasamos la primera fecha
      borderRadius: BorderRadius.circular(15),
    );

    if (pickedDate != null && pickedDate.isNotEmpty && pickedDate[0] != null) {
      setState(() {
        _firstDate = pickedDate[0];
        _secondDate = null;
      });
    }
  }

  // Función para seleccionar la segunda fecha, que debe ser mayor que la primera
  Future<void> _selectSecondDate(BuildContext context) async {
    if (_firstDate == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Selecciona primero la primera fecha.')),
      );
      return;
    }

    final List<DateTime?>? pickedDate = await showCalendarDatePicker2Dialog(
      context: context,
      config: CalendarDatePicker2WithActionButtonsConfig(
        firstDate: _firstDate!.add(const Duration(
            days: 1)), // La segunda fecha debe ser mayor a la primera
        lastDate: DateTime(2101),
        calendarType: CalendarDatePicker2Type.single, // Solo permite una fecha
      ),
      dialogSize: const Size(325, 400),
      value: [_secondDate], // Pasamos la segunda fecha
      borderRadius: BorderRadius.circular(15),
    );

    if (pickedDate != null && pickedDate.isNotEmpty && pickedDate[0] != null) {
      setState(() {
        _secondDate = pickedDate[0];
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Theme.of(context).colorScheme.inversePrimary,
        title: Text(widget.title),
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            const Text(
              'Laboratorios Participantes:',
              textAlign: TextAlign.start,
            ),
            SizedBox(
              height: MediaQuery.of(context).size.height * 0.35,
              child: SingleChildScrollView(
                child: Column(
                  children: [
                    laboratoryBoxCheck(name: 'iOS Lab', id: '1'),
                    laboratoryBoxCheck(name: 'Android Lab', id: '2'),
                    laboratoryBoxCheck(name: 'Flutter Lab', id: '3'),
                    laboratoryBoxCheck(name: 'Web Lab', id: '4'),
                    laboratoryBoxCheck(name: 'Data Science Lab', id: '5'),
                    laboratoryBoxCheck(name: 'iOS Lab', id: '1'),
                    laboratoryBoxCheck(name: 'Android Lab', id: '2'),
                    laboratoryBoxCheck(name: 'Flutter Lab', id: '3'),
                    laboratoryBoxCheck(name: 'Web Lab', id: '4'),
                    laboratoryBoxCheck(name: 'Data Science Lab', id: '5'),
                  ],
                ),
              ),
            ),
            const SizedBox(
              height: 30,
            ),
            const Text(
              'Selecciona las fechas:',
              textAlign: TextAlign.start,
            ),
            const SizedBox(height: 20.0),
            ElevatedButton(
              onPressed: () => _selectFirstDate(context),
              child: const Text('Seleccionar Primera Inicio'),
            ),
            Text(
              _firstDate == null
                  ? 'Primera fecha no seleccionada'
                  : 'Primera fecha: ${_firstDate!.day}/${_firstDate!.month}/${_firstDate!.year}',
            ),
            const SizedBox(height: 20.0),
            ElevatedButton(
              onPressed: () => _selectSecondDate(context),
              child: const Text('Seleccionar Segunda Fin'),
            ),
            Text(
              _secondDate == null
                  ? 'Segunda fecha no seleccionada'
                  : 'Segunda fecha: ${_secondDate!.day}/${_secondDate!.month}/${_secondDate!.year}',
            ),
          ],
        ),
      ),
    );
  }
}

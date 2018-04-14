rollout unnamedRollout "Unity Hair Tool" width:162 height:77
(
	button 'btn1' "Generate" pos:[6,40] width:151 height:30 align:#left
	label 'lbl2' "Segments" pos:[8,12] width:72 height:18 align:#left
	spinner 'spn3' "" pos:[81,12] width:75 height:16 range:[3,25,5] type:#integer align:#left
	on btn1 pressed do
	(
		convertToSplines selection[1] spn3.value
	)
)

createdialog unnamedRollout

fn lerp minVal maxVal term = (maxVal - minVal) * term + minVal

fn generate spline segments = 
(
	vert_array_full = #()
	face_array = #()

	linePath = spline
	for s = 1 to (numSplines linePath) do
	(
		vert_array = #()
		
		for k = 2 to (numKnots linePath s) do
		(
			p1  = getKnotPoint linePath s (k - 1)
			p2  = getKnotPoint linePath s k			
			
			for t = 0 to (segments) do
			(
				p = lerp p1 p2 (1.0*t/segments)
				append vert_array p	
			)
			
		)
		
		step = vert_array.count / segments
		d = step - 1
		for i = 1 to vert_array.count do
		(
			d +=1
			if d == step do
			(
				d = 0
				p = vert_array[i]
				append vert_array_full p
			)
		)		
	)

	append face_array [1,3,2]
	m = mesh vertices:vert_array_full faces:face_array
	m.name = "HairGeometry"
	
	delete spline
)

fn convertToSplines obj segments =
(
	spline = obj.modifiers[#Hair_and_Fur].ConvertGuidesToSplines instance:obj
	generate spline segments
)

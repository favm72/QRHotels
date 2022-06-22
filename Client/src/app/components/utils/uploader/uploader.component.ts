import { Component, ElementRef, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from "@angular/core"
import { faUpload } from "@fortawesome/free-solid-svg-icons/"

@Component({
	selector: "app-uploader",
	templateUrl: "./uploader.component.html",
	styleUrls: ["./uploader.component.css"],
})
export class UploaderComponent implements OnInit, OnChanges {
	@Input() myfile: string = ""
	@Output() myfileChange = new EventEmitter<string>()

	@Input() name: string = ""
	@Output() nameChange = new EventEmitter<string>()

	@ViewChild("fileuploader") fileuploader: ElementRef

	uploadIcon = faUpload
	size = ""
	errorMessage = ""

	constructor() {}
	ngOnChanges(changes: SimpleChanges): void {
		if (changes.myfile && this.fileuploader && changes.myfile.currentValue == "") {
			this.fileuploader.nativeElement.value = null
		}
	}

	ngOnInit(): void {}

	validate(file: File) {
		let splitted = file.name.split(".")
		if (splitted.length != 2) throw Error("Nombre de archivo no debe contener puntos.")
		let ext = splitted[1]
		if (!/(png|jpg)/i.test(ext)) throw Error("Extensión inválida.")
		if (file.size > 1 * 1024 * 1024) throw Error("Límite 1MB.")
	}

	clearValue() {
		this.name = ""
		this.myfile = ""
		this.myfileChange.emit(this.myfile)
		this.nameChange.emit(this.name)
	}

	getsize(bytes: number) {
		let result = ""
		if (bytes >= 1073741824) result = (bytes / 1073741824).toFixed(2) + " GB"
		else if (bytes >= 1048576) result = (bytes / 1048576).toFixed(2) + " MB"
		else if (bytes >= 1024) result = (bytes / 1024).toFixed(2) + " KB"
		else if (bytes > 1) result = bytes + " bytes"
		else if (bytes == 1) result = bytes + " byte"
		else result = "0 bytes"

		return result
	}

	handleChange(event) {
		let files = event.target.files
		if (files && files.length > 0) {
			try {
				let file = files[0]
				this.validate(file)
				let reader = new FileReader()
				reader.onload = e => {
					let route = e.target.result as string
					this.name = file.name
					this.size = this.getsize(file.size)
					this.myfile = route.split(",")[1]
					this.myfileChange.emit(this.myfile)
					this.nameChange.emit(this.name)
				}
				reader.readAsDataURL(file)
				this.errorMessage = ""
			} catch (error) {
				event.target.value = null
				this.clearValue()
				this.errorMessage = error.message
			}
		} else {
			this.size = ""
			event.target.value = null
			this.clearValue()
		}
	}
}

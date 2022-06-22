import { Component, OnInit, ViewChild } from "@angular/core"
import { faEdit } from "@fortawesome/free-solid-svg-icons"
import { DirectoryService } from "src/app/services/directory.service"
import { InfoService } from "src/app/services/info.service"
import { NotificationService } from "src/app/services/notification.service"
import { ModalComponent } from "../utils/modal/modal.component"
import { QuillToolbarConfig } from "ngx-quill"

const initialForm = () => ({
	content: { value: "", invalid: false },
	name: { value: "", invalid: false },
	iconUrl: { value: "", invalid: false },
	bannerUrl: { value: "", invalid: false },
	orderNo: { value: 0, invalid: false },
	image64: { value: "", invalid: false },
	filename: { value: "", invalid: false },
	icon64: { value: "", invalid: false },
	iconname: { value: "", invalid: false },
	active: { value: true, invalid: false },
})

const initialFormHead = () => ({
	introduction: { value: "", invalid: false },
	bannerUrl: { value: "", invalid: false },
	image64: { value: "", invalid: false },
	filename: { value: "", invalid: false },
})

@Component({
	selector: "app-admin-directory",
	templateUrl: "./admin-directory.component.html",
	styleUrls: ["./admin-directory.component.css"],
})
export class AdminDirectoryComponent implements OnInit {
	editIcon = faEdit
	hotels = []
	currentHotel = ""
	currentItem = null
	head = null
	items = []
	form = initialForm()
	formHead = initialFormHead()

	@ViewChild("modalForm") modalForm: ModalComponent
	@ViewChild("modalFormHead") modalFormHead: ModalComponent
	constructor(private directory: DirectoryService, private info: InfoService, private notif: NotificationService) {}

	async ngOnInit(): Promise<void> {
		this.hotels = await this.info.loadHotels()
	}

	get hotelDescription() {
		return this.hotels.find(h => h.id == this.currentHotel)?.description ?? ""
	}

	async loadDirectory() {
		if (this.currentHotel == "") {
			this.items = []
			this.head = null
		} else {
			const data = await this.directory.listAdmin(this.currentHotel)
			this.items = data.detail
			this.head = data.head
		}
	}

	async onChangeHotel() {
		this.loadDirectory()
	}

	addDirectory() {
		this.form = initialForm()
		this.currentItem = null
		this.modalForm.open()
	}

	editDirectory(item) {
		this.form = initialForm()
		this.currentItem = item
		this.form.active.value = item.active
		this.form.name.value = item.name
		this.form.content.value = decodeURIComponent(atob(item.content))
		this.form.orderNo.value = item.orderNo
		this.form.iconUrl.value = item.iconUrl
		this.form.bannerUrl.value = item.bannerUrl
		this.modalForm.open()
	}

	openUpsertModal() {
		this.formHead = initialFormHead()
		if (this.head != null) {
			this.formHead.introduction.value = decodeURIComponent(atob(this.head.introduction))
			this.formHead.bannerUrl.value = this.head.bannerUrl
		}
		this.modalFormHead.open()
	}

	async saveDirectory() {
		const model = {
			name: this.form.name.value,
			content: btoa(encodeURIComponent(this.form.content.value)),
			orderNo: +this.form.orderNo.value,
			image64: this.form.image64.value,
			filename: this.form.filename.value,
			icon64: this.form.icon64.value,
			iconname: this.form.iconname.value,
			active: this.form.active.value,
			hotelCode: this.currentHotel,
			iconUrl: this.form.iconUrl.value,
			bannerUrl: this.form.bannerUrl.value,
		}
		let result = false
		if (this.currentItem == null) result = await this.directory.create(model)
		else result = await this.directory.edit({ ...model, id: this.currentItem.id })

		if (result) {
			this.notif.success(`${this.currentItem == null ? "Registrado" : "Actualizado"} correctamente`)
			this.loadDirectory()
			this.modalForm.close()
			this.form = initialForm()
		}
	}

	async saveDirectoryHead() {
		const model = {
			introduction: btoa(encodeURIComponent(this.formHead.introduction.value)),
			image64: this.formHead.image64.value,
			filename: this.formHead.filename.value,
			hotelCode: this.currentHotel,
		}

		const result = await this.directory.upsertHead(model)

		if (result) {
			this.notif.success("Actualizado correctamente")
			this.modalFormHead.close()
			this.formHead = initialFormHead()
		}
	}
}
